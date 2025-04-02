using AutoMapper;
using MedHelpAuthorizations.Application.Features.Patients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpdatePerson;
using MedHelpAuthorizations.Application.Features.Persons.Commands.CreatePerson;
using MedHelpAuthorizations.Application.Features.Cardholders.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonDto, Person>()
                .ForMember(x => x.AddressId, map => map.MapFrom(y => y.AddressId == 0 ? null : y.AddressId))
                .ForPath(x => x.Id, map => map.MapFrom(y => y.PersonId)).ReverseMap();

            CreateMap<UpdatePersonCommand, Person>()
                .ForMember(x => x.AddressId, map => map.MapFrom(y => y.AddressId == 0 ? null : y.AddressId))
                .ForPath(x => x.Id, map => map.MapFrom(y => y.PersonId)).ReverseMap();

            CreateMap<CreatePersonCommand, Person>()
                .ForMember(x => x.AddressId, map => map.MapFrom(y => y.AddressId == 0 ? null : y.AddressId))
                .ForPath(x => x.Id, map => map.MapFrom(y => y.PersonId)).ReverseMap();

            CreateMap<UpsertPersonCommand, Person>()
                .ForMember(x => x.AddressId, map => map.MapFrom(y => y.AddressId == 0 ? null : y.AddressId))
                .ForPath(x => x.Id, map => map.MapFrom(y => y.PersonId)).ReverseMap();

            CreateMap<UpsertPersonCommand, AddEditPatientCommand>().ReverseMap();
            CreateMap<UpsertPersonCommand, AddEditProviderCommand>().ReverseMap();

            CreateMap<UpsertPersonCommand, UpdatePersonCommand>().ReverseMap();

            CreateMap<UpsertPersonCommand, CreatePersonCommand>().ReverseMap();

            CreateMap<AddEditCardholderCommand, UpsertPersonCommand>().ReverseMap();
        }
    }
}
