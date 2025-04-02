using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Queries.GetAllPagedData;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientEncounterTypeProfile : Profile
    {
        public ClientEncounterTypeProfile()
        {
            CreateMap<AddEditClientEncounterTypeCommand, ClientEncounterType>().ReverseMap();
            CreateMap<GetAllPagedClientEncounterTypeQuery, ClientEncounterType>().ReverseMap();
        }
    }
}
