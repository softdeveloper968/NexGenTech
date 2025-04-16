using AutoMapper;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByPatientId;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class InsuranceCardProfile : Profile
    {
        public InsuranceCardProfile()
        {
            CreateMap<InsuranceCard, GetInsuranceCardsByPatientIdResponse>()
                .ForPath(x => x.CardholderLastName, map => map.MapFrom(y => y.Cardholder.Person.LastName))
                .ForPath(x => x.CardholderFirstName, map => map.MapFrom(y => y.Cardholder.Person.FirstName))
                .ForPath(x => x.DateOfBirth, map => map.MapFrom(y => y.Cardholder.Person.DateOfBirth))
                .ForPath(x => x.CardholderMiddleName, map => map.MapFrom(y => y.Cardholder.Person.MiddleName))
                .ReverseMap();
                //.ForPath(x => x.PHONE, map => map.MapFrom(y => y.Cardholder.Person.HomePhoneNumber))
                //.ForPath(x => x.MobilePhoneNumber, map => map.MapFrom(y => y.Cardholder.Person.MobilePhoneNumber))
                //.ForPath(x => x.OfficePhoneNumber, map => map.MapFrom(y => y.Cardholder.Person.OfficePhoneNumber))
                //.ForPath(x => x.Email, map => map.MapFrom(y => y.Cardholder.Person.Email))
                //.ForPath(x => x.AddressId, map => map.MapFrom(y => y.Cardholder.Person.AddressId))
                //.ForPath(x => x.Address.AddressStreetLine1, map => map.MapFrom(y => y.Cardholder.Person.Address.AddressStreetLine1))
                //.ForPath(x => x.Address.AddressStreetLine2, map => map.MapFrom(y => y.Cardholder.Person.Address.AddressStreetLine2))
                //.ForPath(x => x.Address.City, map => map.MapFrom(y => y.Cardholder.Person.Address.City))
                //.ForPath(x => x.Address.StateId, map => map.MapFrom(y => y.Cardholder.Person.Address.StateId))
                //.ForPath(x => x.Address.PostalCode, map => map.MapFrom(y => y.Cardholder.Person.Address.PostalCode)).ReverseMap();
            CreateMap<AddEditInsuranceCardCommand, GetInsuranceCardsByPatientIdResponse>().ReverseMap();
            CreateMap<AddEditInsuranceCardCommand, InsuranceCard>().ReverseMap();
        }
    }
}
