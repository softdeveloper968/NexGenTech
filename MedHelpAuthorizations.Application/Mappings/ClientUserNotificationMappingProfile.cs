using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Commands;
using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetAllPaged;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientUserNotificationMappingProfile : Profile
    {
        public ClientUserNotificationMappingProfile()
        {
            CreateMap<ClientUserNotification, AddEditClientUserNotificationCommand>().ReverseMap();
            CreateMap<AddEditClientUserNotificationCommand, ClientUserNotification>().ReverseMap();
            CreateMap<GetAllClientUserNotificationResponse, ClientUserNotification>().ReverseMap();
        }
    }
}
