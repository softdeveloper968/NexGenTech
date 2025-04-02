using AutoMapper;
using MedHelpAuthorizations.Shared.Models.EligibilityExcelExportHelper;
using self_pay_eligibility_api.Application.Features.Eligibility.DiscoveredEligibility.Base;
using self_pay_eligibility_api.Application.Features.Eligibility.DiscoveredEligibility.Queries.GetByCriteria;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetByCriteria;

namespace MedHelpAuthorizations.Infrastructure.Mappings
{
    /// <summary>
    /// Profile class that defines mapping configurations between data transfer objects (DTOs) and view models
    /// for generating Excel reports of eligibility check results. It specifies how to map properties
    /// from the source DTOs to the destination view models, allowing for structured Excel exports.
    /// </summary> //AA-300
    public class EligibilityExcelExportHelperProfile : Profile
    {
        public EligibilityExcelExportHelperProfile()
        { 

            CreateMap<GetEligibilityCheckRequestByCriteriaResponse, EligibilityCheckReportModel>().ReverseMap();
                //.ForMember(dest => dest.ClientLocationName, opt => opt.MapFrom(src => src.ClientLocationName))
                //.ForMember(dest => dest.ProviderName, opt => opt.MapFrom(src => src.ProviderName))
                //.ForMember(dest => dest.PatientFirstName, opt => opt.MapFrom(src => src.PatientFirstName))
                //.ForMember(dest => dest.PatientLastName, opt => opt.MapFrom(src => src.PatientLastName))
                //// Add null check using NullSubstitute
                //.ForMember(dest => dest.PatientMiddleName, opt => opt.MapFrom(src => src.PatientMiddleName))
                //.ForMember(dest => dest.PatientDateOfBirth, opt => opt.MapFrom(src => src.PatientDateOfBirth))
                //.ForMember(dest => dest.PatientSocialSecurityNumber, opt => opt.MapFrom(src => src.PatientSocialSecurityNumber))
                //.ForMember(dest => dest.DateOfService, opt => opt.MapFrom(src => src.DateOfService))
                //.ForMember(dest => dest.LkpClientInsuranceName, opt => opt.MapFrom(src => src.LkpClientInsuranceName))
                //.ForMember(dest => dest.LkpMemberNumber, opt => opt.MapFrom(src => src.LkpMemberNumber))
                //.ForMember(dest => dest.LkpGroupNumber, opt => opt.MapFrom(src => src.LkpGroupNumber))
                //.ForMember(dest => dest.LkpSubscriberLastName, opt => opt.MapFrom(src => src.LkpSubscriberLastName))
                //.ForMember(dest => dest.LkpSubscriberFirstName, opt => opt.MapFrom(src => src.LkpSubscriberFirstName))
                //.ForMember(dest => dest.LkpSubscriberDateOfBirth, opt => opt.MapFrom(src => src.LkpSubscriberDateOfBirth))
                //.ForMember(dest => dest.EligibilityCheckCompletedOn, opt => opt.MapFrom(src => src.EligibilityCheckCompletedOn))
                

            CreateMap<GetDiscoveredEligibilityBaseResponse, EligibilityCheckReportModel>()
                .ForMember(dest => dest.CardholderNameAndAddress, opt => opt.MapFrom(src => $"{src.Cardholder.FirstName} {src.Cardholder.LastName}/{src.Cardholder.Address.AddressStreetLine1},{src.Cardholder.Address.AddressStreetLine2},{src.Cardholder.Address.City},{src.Cardholder.Address.PostalCode}"))
                .ReverseMap();

                //.ForMember(dest => dest.ClientLocationName, opt => opt.MapFrom(src => src.ClientLocationName))
                //.ForMember(dest => dest.PatientFirstName, opt => opt.MapFrom(src => src.PatientFirstName))
                //.ForMember(dest => dest.PatientLastName, opt => opt.MapFrom(src => src.PatientLastName))
                //.ForMember(dest => dest.PatientMiddleName, opt => opt.MapFrom(src => src.PatientMiddleName))
                //.ForMember(dest => dest.PatientDateOfBirth, opt => opt.MapFrom(src => src.PatientDateOfBirth))
                //.ForMember(dest => dest.LkpClientInsuranceName, opt => opt.MapFrom(src => src.LkpClientInsuranceName))
                //.ForMember(dest => dest.LkpMemberNumber, opt => opt.MapFrom(src => src.LkpMemberNumber))
                //.ForMember(dest => dest.LkpGroupNumber, opt => opt.MapFrom(src => src.LkpGroupNumber))
                //.ForMember(dest => dest.LkpSubscriberLastName, opt => opt.MapFrom(src => src.LkpSubscriberLastName))
                //.ForMember(dest => dest.LkpSubscriberFirstName, opt => opt.MapFrom(src => src.LkpSubscriberFirstName))
                //.ForMember(dest => dest.LkpSubscriberDateOfBirth, opt => opt.MapFrom(src => src.LkpSubscriberDateOfBirth))
                //.ForMember(dest => dest.EligibilityCheckCompletedOn, opt => opt.MapFrom(src => src.EligibilityCheckCompletedOn))
                //.ForMember(dest => dest.PayerName, opt => opt.MapFrom(src => src.Payer.PayerName))
                //.ForMember(dest => dest.MemberNumber, opt => opt.MapFrom(src => src.MemberNumber))
                //.ForMember(dest => dest.GroupNumber, opt => opt.MapFrom(src => src.GroupNumber))
                //.ForMember(dest => dest.Copay, opt => opt.MapFrom(src => src.Copay))
                //.ForMember(dest => dest.Deductible, opt => opt.MapFrom(src => src.Deductible))
                //.ForMember(dest => dest.RemainingDeductible, opt => opt.MapFrom(src => src.RemainingDeductible));

        }
    }
}
