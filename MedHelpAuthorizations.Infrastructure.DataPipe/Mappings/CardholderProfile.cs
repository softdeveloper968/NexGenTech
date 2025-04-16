using AutoMapper;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.DataPipe.Models;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Mappings
{
    public class CardholderProfile : Profile
    {
        public CardholderProfile()
        {
            CreateMap<TblCardHolder, Cardholder>()
                //.ForPath(x => x.Id, map => map.MapFrom(y => y.CardholderId))
                //.ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.CardholderLastName))
                //.ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.CardholderFirstName))
                //.ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
                //.ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
                //.ForPath(x => x.Person.MiddleName, map => map.MapFrom(y => y.CardholderMiddleName))
                //.ForPath(x => x.Person.HomePhoneNumber, map => map.MapFrom(y => y.HomePhoneNumber))
                //.ForPath(x => x.Person.MobilePhoneNumber, map => map.MapFrom(y => y.MobilePhoneNumber))
                //.ForPath(x => x.Person.OfficePhoneNumber, map => map.MapFrom(y => y.OfficePhoneNumber))
                //.ForPath(x => x.Person.Email, map => map.MapFrom(y => y.Email))
                //.ForPath(x => x.Person.AddressId, map => map.MapFrom(y => y.AddressId))
                //.ForPath(x => x.Person.Address.AddressStreetLine1, map => map.MapFrom(y => y.AddressStreetLine1))
                //.ForPath(x => x.Person.Address.AddressStreetLine2, map => map.MapFrom(y => y.AddressStreetLine2))
                //.ForPath(x => x.Person.Address.City, map => map.MapFrom(y => y.City))
                //.ForPath(x => x.Person.Address.StateId, map => map.MapFrom(y => y.StateId))
                //.ForPath(x => x.Person.Address.PostalCode, map => map.MapFrom(y => y.PostalCode))
                .ReverseMap();
            CreateMap<TblCardHolder, PersonDto>()
                //.ForPath(x => x.Id, map => map.MapFrom(y => y.CardholderId))
                //.ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.CardholderLastName))
                //.ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.CardholderFirstName))
                //.ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
                //.ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
                //.ForPath(x => x.Person.MiddleName, map => map.MapFrom(y => y.CardholderMiddleName))
                //.ForPath(x => x.Person.HomePhoneNumber, map => map.MapFrom(y => y.HomePhoneNumber))
                //.ForPath(x => x.Person.MobilePhoneNumber, map => map.MapFrom(y => y.MobilePhoneNumber))
                //.ForPath(x => x.Person.OfficePhoneNumber, map => map.MapFrom(y => y.OfficePhoneNumber))
                //.ForPath(x => x.Person.Email, map => map.MapFrom(y => y.Email))
                //.ForPath(x => x.Person.AddressId, map => map.MapFrom(y => y.AddressId))
                //.ForPath(x => x.Person.Address.AddressStreetLine1, map => map.MapFrom(y => y.AddressStreetLine1))
                //.ForPath(x => x.Person.Address.AddressStreetLine2, map => map.MapFrom(y => y.AddressStreetLine2))
                //.ForPath(x => x.Person.Address.City, map => map.MapFrom(y => y.City))
                //.ForPath(x => x.Person.Address.StateId, map => map.MapFrom(y => y.StateId))
                //.ForPath(x => x.Person.Address.PostalCode, map => map.MapFrom(y => y.PostalCode))
                .ReverseMap();
        }
    }
}
