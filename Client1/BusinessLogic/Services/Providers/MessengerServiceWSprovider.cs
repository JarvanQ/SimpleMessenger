using BusinessLogic.IServices;

namespace BusinessLogic.Services.Providers
{
    public class MessengerServiceWSprovider
    {
        private readonly IEnvironmentsProvider _environmentsProvider;
        public MessengerServiceWSprovider(IEnvironmentsProvider environmentsProvider) 
        {
            _environmentsProvider = environmentsProvider;
        }


        public async Task<bool> SaveMessageNumberСarefully(string messageNumber)
        {
            bool result = false;
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
            await semaphoreSlim.WaitAsync();
            try
            {
               result = await SaveMessageNumberAsync(messageNumber);
            }
            finally
            {
                semaphoreSlim.Release();
            }

            return result;
        }

        /// <summary>
        /// Сохраняем запись какой последний номер отправляли на сервер
        /// </summary>
        /// <param name="newNumber"></param>
        /// <returns></returns>
        private bool SaveMessageNumber(string newNumber)
        {
            try
            {
                using (FileStream fs = new FileStream(_environmentsProvider.MessageNumberStorage,
                    FileMode.Truncate,
                    FileAccess.ReadWrite,
                    FileShare.None))
                {
                    StreamWriter writer = new StreamWriter(fs);
                    writer.WriteLine(newNumber);
                    writer.Flush();
                    fs.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                //TODO:LOG
                return false;
            }
        }

        private async Task<bool> SaveMessageNumberAsync(string newNumber)
        {
            bool res = await Task.Run(() => SaveMessageNumber(newNumber));
            return res;
        }



    }
}
