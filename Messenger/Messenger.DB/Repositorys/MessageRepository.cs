using Messenger.Core.Entitys;
using Messenger.Core.IRepository;
using NLog;
using Npgsql;

namespace Messenger.DB.Repositorys
{
    public class MessageRepository : IMessageRepository
    {
        private static readonly ILogger logger = LogManager.GetLogger(nameof(MessageRepository));
        private readonly string _connectionString;
        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление записи в таблицу messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<int?> Add(Message message)
        {

            return await Task<int?>.Run(() => 
            {
                int? newId = null;
                try 
                {
                    
                    using (NpgsqlConnection server = new NpgsqlConnection(_connectionString))
                    {
                        server.Open();
                        NpgsqlCommand insertCommand = new NpgsqlCommand();
                        insertCommand.Connection = server;
                        insertCommand.CommandText = "INSERT INTO messages (number,arrivaltime,kontent) VALUES (@Number, @ArrivalTime, @Kontent) RETURNING id;";
                        insertCommand.Parameters.AddWithValue("@Number", NpgsqlTypes.NpgsqlDbType.Integer, message.Number);
                        insertCommand.Parameters.AddWithValue("@ArrivalTime", NpgsqlTypes.NpgsqlDbType.TimestampTz,message.ArrivalTime );
                        insertCommand.Parameters.AddWithValue("@Kontent", NpgsqlTypes.NpgsqlDbType.Varchar, message.Kontent);

                        NpgsqlDataReader queryData = insertCommand.ExecuteReader();
                        if (queryData.Read()) 
                        {
                            newId = queryData.GetInt32(0);
                        }

                        insertCommand.Dispose();
                        server.Close();
                    }
                    return newId;
                }
                catch(Exception ex)
                {
                    logger.Error(ex,"Ошибка добавление записи в таблицу");
                    return null;
                }

            });

        }

        /// <summary>
        /// Получение списка сообщений за отрезок времени
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async Task<List<Message>> GetListByTimeInterval(DateTime start, DateTime end)
        {
            try
            {
                List<Message> messages = new List<Message>();
                using (NpgsqlConnection server = new NpgsqlConnection(_connectionString))
                {
                    server.Open();
                   
                    NpgsqlCommand selectCommand = new NpgsqlCommand();
                    selectCommand.Connection = server;
                     selectCommand.CommandText = "SELECT id, number, kontent, arrivaltime FROM messages " +
                       "WHERE arrivaltime> @DateFrom AND arrivaltime < @DateTo;";

                     selectCommand.Parameters.AddWithValue("@DateFrom", NpgsqlTypes.NpgsqlDbType.TimestampTz, start);
                     selectCommand.Parameters.AddWithValue("@DateTo", NpgsqlTypes.NpgsqlDbType.TimestampTz, end);

                    NpgsqlDataReader queryData = await selectCommand.ExecuteReaderAsync();
                    while (queryData.Read())
                    {
                        Message _message = new Message();
                        _message.Id = queryData.GetInt32(0);
                        _message.Number = queryData.GetInt32(1);
                        _message.Kontent = queryData.GetString(2);
                        _message.ArrivalTime = queryData.GetDateTime(3);

                        messages.Add(_message);
                    }

                    selectCommand.Dispose();
                    server.Close();
                }
                
                return messages;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Не удалось счтать записи");
                return null;
            }
        }

     
    }
}
