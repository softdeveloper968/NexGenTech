using AutoMapper;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Features.Addresses.ViewModels;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.UpsertAddresses;
using MedHelpAuthorizations.Application.Features.Patients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.UpdateAddress;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.CreateAddress;
using MedHelpAuthorizations.Application.Features.Cardholders.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Commands.AddEdit;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>().ReverseMap();

            CreateMap<GetAddressesViewModel, Address>()
                .ForPath(x => x.Id, map => map.MapFrom(y => y.AddressId)).ReverseMap();

            CreateMap<UpsertAddressCommand, AddEditPatientCommand>().ReverseMap();
            
            CreateMap<UpsertAddressCommand, AddEditCardholderCommand>().ReverseMap();

            //AA-95
            CreateMap<UpsertAddressCommand, Address>().ForPath(x => x.Id, map => map.MapFrom(y => y.AddressId)).ReverseMap();

            
            CreateMap<UpsertAddressCommand, AddEditProviderCommand>().ReverseMap();
            
            CreateMap<UpsertAddressCommand, UpdateAddressCommand>().ReverseMap();

            CreateMap<UpsertAddressCommand, CreateAddressCommand>().ReverseMap();
            
            CreateMap<Address, CreateAddressCommand>()
                .ForPath(x => x.AddressId, map => map.MapFrom(y => y.Id)).ReverseMap();
            
            CreateMap<Address, UpdateAddressCommand>()
                .ForPath(x => x.AddressId, map => map.MapFrom(y => y.Id)).ReverseMap();
        }
    }
}
