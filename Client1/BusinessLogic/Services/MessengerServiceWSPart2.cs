using Data.DTO;

namespace BusinessLogic.Services
{
    public partial class MessengerServiceWS
    {
        
        /// <summary>
        /// Сенерировать новое сообщение 
        /// со случайным текстом
        /// </summary>
        /// <returns></returns>
        private MessageDTO GenerateRandomMessage()
        {
            MessageDTO newMessage = new MessageDTO();

            Random random = new Random();
            int randomLength = random.Next(128);
            int number = ReadMessageNumber() + 1;
            newMessage.Number = number.ToString();
            newMessage.Kontent = Faker.StringFaker.AlphaNumeric(randomLength);


            return newMessage;
        }

        private int ReadMessageNumber()
        {
            try
            {
                using (FileStream fs = new FileStream(_environmentsProvider.MessageNumberStorage,
                    FileMode.OpenOrCreate,
                    FileAccess.Read,
                    FileShare.None))
                {
                    fs.Position = 0;
                    StreamReader reader = new StreamReader(fs);
                    string content = reader.ReadToEnd();

                    fs.Close();
                    bool flag = int.TryParse(content, out int result);
                    return flag ? result : 0;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex,"Не удалось считать с айла номер сообщения!!!!!!");
                return 0;
            }
        }




    }
}
