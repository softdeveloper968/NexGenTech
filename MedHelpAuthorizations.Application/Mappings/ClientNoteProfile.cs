using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Queries.GetAllPagedData;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientNoteProfile : Profile
    {
        public ClientNoteProfile()
        {
            CreateMap<AddEditClientNoteCommand, ClientNote>().ReverseMap();
            CreateMap<GetAllPagedClientNotesQuery, ClientNote>().ReverseMap();
            CreateMap<GetAllPagedClientNotesResponse, ClientNote>().ReverseMap();
        }
    }
}
