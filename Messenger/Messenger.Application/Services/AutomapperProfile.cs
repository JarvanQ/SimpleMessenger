using AutoMapper;
using Messenger.Application.DTO;
using Messenger.Core.Entitys;


namespace Messenger.Application.Services
{
    /// <summary>
    /// Профиль для автомаппера
    /// </summary>
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        {
            CreateMap<MessageDTO, Message>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id == null ? 0 : (int)src.Id));

            CreateMap<Message, MessageDTO>();

            CreateMap<NewMessageDTO,Message>()
                .ForMember(x => x.Kontent, opt => opt.MapFrom(src => src.Kontent))
                .AfterMap((src, m) =>
            {
                m.Id = 0;

            });
        }




        
    }
}
