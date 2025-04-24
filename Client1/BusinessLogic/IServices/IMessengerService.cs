using Data.DTO;


namespace BusinessLogic.IServices
{
    public interface IMessengerService
    {

        /// <summary>
        /// Отправить на сервер сообщение
        /// </summary>
        /// <returns>id сообщения после сохранения</returns>
        public Task SendMessageWS(MessageDTO message);


        /// <summary>
        /// 1)генерирует сообщение
        /// 2)отправлет его на сервер
        /// 3)обрабатывает
        /// 4)сохраняет его номер
        /// </summary>
        /// <returns></returns>
        public Task<bool> SendMessage();


    }
}
