using Messenger.Application.IServices;
using Messenger.Core;
using Messenger.Core.IRepository;
using Messenger.DB.Repositorys;
using NLog;
using Npgsql;


namespace Messenger.DB
{

    public class DatabaseService : IDatabaseService
    {

        private readonly IEnvironmentProvider _options;
        private readonly string _dbConnectionString;
        private readonly string _serverConnectionString;

        private static readonly ILogger logger = LogManager.GetLogger(nameof(DatabaseService));
        public IMessageRepository MessageRepository { get; private set; }

 
        public DatabaseService(IEnvironmentProvider options)
        {
            _options = options;
            _serverConnectionString = _options.DbServerConnectionString;
            _dbConnectionString = _options.DbConnectionString;

            InitRepositorys(_dbConnectionString);
        }

        public void InitRepositorys(string connectionString)
        {
            MessageRepository = new MessageRepository(connectionString);
        }

       
        /// <summary>
        /// Инициализация БД и таблицы
        /// </summary>
        /// <returns></returns>
        public bool InitDB()
        {
            try
            {
                using (NpgsqlConnection server = new NpgsqlConnection(_serverConnectionString))
                {
                    server.Open();

                    NpgsqlCommand createBD
                    = new NpgsqlCommand("CREATE DATABASE testdb  ENCODING = 'UTF8' CONNECTION LIMIT = -1;", server);
                    createBD.ExecuteNonQuery();
                    server.Close();
                }
                logger.Info("Создали БД");
            }
            catch (Npgsql.PostgresException ex) 
            {
                //допускаеться ошибка попытки создать существующую БД
                var exception = (PostgresException)ex;
                if (exception.SqlState != "42P04")
                {
                    logger.Fatal(ex, "Ошибка при инициализации БД.Не удалось создать таблицу messages. ");
                    return false;
                }
                logger.Info("Повторная попытка создать БД");
            }

            try
            {
                using (NpgsqlConnection server = new NpgsqlConnection(_dbConnectionString )) 
                {
                    server.Open();
                    string createTBtext = "CREATE TABLE IF NOT EXISTS messages " +
                        "(id SERIAL PRIMARY KEY," +
                        "number int," +
                        "kontent character varying(128)," +
                        "arrivalTime timestamptz);";
                    NpgsqlCommand createTB
                        = new NpgsqlCommand(createTBtext, server);
                    createTB.ExecuteNonQuery();
                    server.Close();
                }

                logger.Info("Таблица messages есть");
            }

            catch (Exception ex)
            {
               logger.Fatal(ex, "Ошибка при инициализации БД.Не удалось создать таблицу messages. ");
               return false;
            }

            return true;


        }


    }
}
