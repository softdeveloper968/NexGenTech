using AutoMapper;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetBySearchString;
using MedHelpAuthorizations.Application.Features.Patients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<GetPatientsBySearchStringResponse, Patient>()
               .ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.LastName))
               .ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.FirstName))
               .ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
               .ForPath(x => x.AdministrativeGenderId, map => map.MapFrom(y => y.AdministrativeGenderId))
               .ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
               .ForPath(x => x.Person.MiddleName, map => map.MapFrom(y => y.MiddleName))
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
               .ForMember(x => x.ResponsiblePartyId, map => map.MapFrom(y => y.ResponsiblePartyId))
               .ForMember(x => x.ResponsiblePartyRelationshipToPatient, map => map.MapFrom(y => y.ResponsiblePartyRelationshipToPatient))
               .ForPath(x => x.Person.SocialSecurityNumber, map => map.MapFrom(y => y.SocialSecurityNumber)) 
            .ReverseMap();

            CreateMap<GetPatientsByCriteriaResponse, Patient>()
                .ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.LastName))
                .ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.FirstName))
                .ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
                .ForPath(x => x.AdministrativeGenderId, map => map.MapFrom(y => y.AdministrativeGenderId))
                .ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
                .ForPath(x => x.Person.MiddleName, map => map.MapFrom(y => y.MiddleName))
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
                .ForMember(x => x.ResponsiblePartyId, map => map.MapFrom(y => y.ResponsiblePartyId))
                .ForMember(x => x.ResponsiblePartyRelationshipToPatient, map => map.MapFrom(y => y.ResponsiblePartyRelationshipToPatient))
                .ForPath(x => x.Person.SocialSecurityNumber, map => map.MapFrom(y => y.SocialSecurityNumber)) //AA-218
            .ReverseMap();

            CreateMap<AddEditPatientCommand, Patient>()
                .ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.LastName))
                .ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.FirstName))
                .ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
                .ForPath(x => x.AdministrativeGenderId, map => map.MapFrom(y => y.AdministrativeGenderId))
                .ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
                .ReverseMap();

            CreateMap<GetPatientByIdResponse, Patient>()
                .ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.LastName))
                .ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.FirstName))
                .ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
                .ForPath(x => x.AdministrativeGenderId, map => map.MapFrom(y => y.AdministrativeGenderId))
                .ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
                .ForPath(x => x.Person.MiddleName, map => map.MapFrom(y => y.MiddleName))
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
                .ForMember(x => x.ResponsiblePartyId, map => map.MapFrom(y => y.ResponsiblePartyId))
                .ForMember(x => x.ResponsiblePartyRelationshipToPatient, map => map.MapFrom(y => y.ResponsiblePartyRelationshipToPatient))
                .ForPath(x => x.Person.SocialSecurityNumber, map => map.MapFrom(y => y.SocialSecurityNumber)) //AA-218
            .ReverseMap();

            CreateMap<GetAllPagedPatientsResponse, Patient>()
                .ForPath(x => x.Person.LastName, map => map.MapFrom(y => y.LastName))
                .ForPath(x => x.Person.FirstName, map => map.MapFrom(y => y.FirstName))
                .ForPath(x => x.Person.GenderIdentityId, map => map.MapFrom(y => y.GenderIdentityId))
                .ForPath(x => x.AdministrativeGenderId, map => map.MapFrom(y => y.AdministrativeGenderId))
                .ForPath(x => x.Person.DateOfBirth, map => map.MapFrom(y => y.DateOfBirth))
                .ForPath(x => x.Person.MiddleName, map => map.MapFrom(y => y.MiddleName))
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
                .ForMember(x => x.ResponsiblePartyId, map => map.MapFrom(y => y.ResponsiblePartyId))
                .ForMember(x => x.ResponsiblePartyRelationshipToPatient, map => map.MapFrom(y => y.ResponsiblePartyRelationshipToPatient))
            .ReverseMap();
        }
    }
}
