

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Messenger.Application.DTO
{
    [SwaggerSchema("SwaggerSchema.Description on class", ReadOnly = true, Nullable = false)]
    public class NewMessageDTO
    {
        /// <summary>
        /// Номер
        /// </summary>
        [SwaggerSchema("Номер сообщения")]
        [DefaultValue("777")]
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Содержимое 
        /// </summary>
        [SwaggerSchema("Содержимое")]
        [DefaultValue("Kontent")]
        public string Kontent { get; set; } = string.Empty;

        public bool Valid() 
        {
            if (string.IsNullOrEmpty(Kontent) || string.IsNullOrWhiteSpace(Kontent)||
                !int.TryParse(Number, out int number) || number<0   )
            {
                return false;
            }
            return true;

        }

    }
}
