using AutoMapper;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetAllPaged;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetByCriteria;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetById;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ResponsiblePartyProfile : Profile
    {
        public ResponsiblePartyProfile()
        {
            CreateMap<Address, AddEditResponsiblePartyCommand>()
                .ReverseMap();                

            CreateMap<Person, AddEditResponsiblePartyCommand>()
                .ReverseMap();

            CreateMap<AddEditResponsiblePartyCommand, ResponsibleParty>()
                .ForPath(x => x.Person.Address, map => map.MapFrom(y => y))
                .ForMember(x => x.Person, map => map.MapFrom(y => y))
                .ReverseMap();
            

            CreateMap<Person, GetAllPagedResponsiblePartyResponse>()
               .ReverseMap();

            CreateMap<GetAllPagedResponsiblePartyResponse, ResponsibleParty>()                 
                .ForMember(x => x.Person, map => map.MapFrom(y => y))
                .ReverseMap();

            CreateMap<Person, GetByCritieriaResponsiblePartyResponse>()
                .ReverseMap();

            CreateMap<GetByCritieriaResponsiblePartyResponse, ResponsibleParty>()
                .ForMember(x => x.Person, map => map.MapFrom(y => y))
                .ReverseMap();

            CreateMap<Person, GetResponsiblePartyByIdResponse>()
               .ReverseMap();

            CreateMap<GetResponsiblePartyByIdResponse, ResponsibleParty>()
                .ForMember(x => x.Person, map => map.MapFrom(y => y))
                .ReverseMap();
        }
    }
}
