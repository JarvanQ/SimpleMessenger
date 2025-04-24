using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.IServices
{
    public interface IEnvironmentsProvider
    {
        /// <summary>
        /// webSocket-адресс сервера
        /// </summary>
        public string ServerWsUrl { get; }

        public string LogsPath { get; }
    }
}
