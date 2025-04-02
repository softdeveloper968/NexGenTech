using AutoMapper;
using MedHelpAuthorizations.Application.Features.Cardholders.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.CardholderViewModels;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByPatientId;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class CardholderProfile : Profile
    {
        public CardholderProfile()
        {
            CreateMap<CardholderViewModel, Cardholder>().ReverseMap();
            CreateMap<AddEditCardholderCommand, Cardholder>().ReverseMap();
            CreateMap<CardholderViewModel, Cardholder>()
                .ForPath(x => x.Id, map => map.MapFrom(y => y.CardholderId))
                .ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.CardholderLastName))
                .ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.CardholderFirstName))
                .ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
                .ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
                .ForPath(x => x.Person.MiddleName, map => map.MapFrom(y => y.CardholderMiddleName))
                .ForPath(x => x.Person.HomePhoneNumber, map => map.MapFrom(y => y.HomePhoneNumber))
                .ForPath(x => x.Person.MobilePhoneNumber, map => map.MapFrom(y => y.MobilePhoneNumber))
                .ForPath(x => x.Person.OfficePhoneNumber, map => map.MapFrom(y => y.OfficePhoneNumber))
                .ForPath(x => x.Person.Email, map => map.MapFrom(y => y.Email))
                .ForPath(x => x.Person.AddressId, map => map.MapFrom(y => y.AddressId))
                .ForPath(x => x.Person.Address.AddressStreetLine1, map => map.MapFrom(y => y.AddressStreetLine1))
                .ForPath(x => x.Person.Address.AddressStreetLine2, map => map.MapFrom(y => y.AddressStreetLine2))
                .ForPath(x => x.Person.Address.City, map => map.MapFrom(y => y.City))
                .ForPath(x => x.Person.Address.StateId, map => map.MapFrom(y => y.StateId))
                .ForPath(x => x.Person.Address.PostalCode, map => map.MapFrom(y => y.PostalCode))
                .ReverseMap();
            CreateMap<CardholderViewModel, AddEditCardholderCommand>()
                .ForPath(x => x.Id, map => map.MapFrom(y => y.CardholderId))
                .ForPath(x => x.LastName, map => map.MapFrom(y => y.CardholderLastName))
                .ForPath(x => x.FirstName, map => map.MapFrom(y => y.CardholderFirstName))
                .ForPath(x => x.MiddleName, map => map.MapFrom(y => y.CardholderMiddleName))
                .ReverseMap();
            CreateMap<CardholderViewModel, GetInsuranceCardsByPatientIdResponse>().ReverseMap();
            CreateMap<CardholderViewModel, GetCardholdersByCriteriaQuery>()
                .ForPath(x => x.Id, map => map.MapFrom(y => y.CardholderId))
                .ForPath(x => x.LastName, map => map.MapFrom(y => y.CardholderLastName))
                .ForPath(x => x.FirstName, map => map.MapFrom(y => y.CardholderFirstName))
                .ForPath(x => x.BirthDate, map => map.MapFrom(y => y.DateOfBirth))             
                .ReverseMap();
			// CreateMap<Cardholder, GetAllPagedCardholdersResponse>().ReverseMap();
			// CreateMap<GetCardholderResponseBase, GetAllPagedCardholdersResponse>().ReverseMap();
			//CreateMap<GetCardholderResponseBase, GetCardholderByIdResponse>().ReverseMap();
			//CreateMap<GetCardholderResponseBase, GetCardholdersByCriteriaResponse>().ReverseMap();
		}
    }
}
