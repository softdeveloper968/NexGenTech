using AutoMapper;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Base;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetById;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetUnresolvedByBatchId;

    public class ClaimStatusBatchClaimProfile : Profile
    {
        public ClaimStatusBatchClaimProfile()
        {
            CreateMap<ClaimStatusBatchClaim, GetClaimStatusBatchClaimBaseResponse>()
                //.ForPath(x => x.CurrentLineItemStatusId, map => map.MapFrom(src => src.ClaimStatusTransaction != null ? src.ClaimStatusTransaction.ClaimLineItemStatusId : null))
                .ForPath(x => x.LastStatusChangedOn, map => map.MapFrom(src => src.ClaimStatusTransaction != null && src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ != null ? src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn : null))
                .ForPath(x => x.RenderingNpi, map => map.MapFrom(src => src.ClientProvider != null ? src.ClientProvider.Npi : src.RenderingNpi))
                .ForPath(x => x.PatientFirstName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.FirstName : string.Empty : string.Empty))
                .ForPath(x => x.PatientLastName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.LastName : string.Empty : string.Empty))
                .ForPath(x => x.DateOfBirth, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.DateOfBirth : new() : new()))
                .ForPath(x => x.ClientLocationNpi, map => map.MapFrom(src => src.ClientLocation != null ? src.ClientLocation.Npi : string.Empty))
                .ReverseMap();

            CreateMap<ClaimStatusBatchClaim, GetAllClaimStatusBatchClaimsResponse>()
                //.ForPath(x => x.CurrentLineItemStatusId, map => map.MapFrom(src => src.ClaimStatusTransaction != null ? src.ClaimStatusTransaction.ClaimLineItemStatusId : null))
                .ForPath(x => x.LastStatusChangedOn, map => map.MapFrom(src => src.ClaimStatusTransaction != null && src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ != null ? src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn : null))
                .ForPath(x => x.RenderingNpi, map => map.MapFrom(src => src.ClientProvider != null ? src.ClientProvider.Npi : src.RenderingNpi))
                .ForPath(x => x.PatientFirstName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.FirstName : string.Empty : string.Empty))
                .ForPath(x => x.PatientLastName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.LastName : string.Empty : string.Empty))
                .ForPath(x => x.DateOfBirth, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.DateOfBirth : new() : new()))
                .ForPath(x => x.ClientLocationNpi, map => map.MapFrom(src => src.ClientLocation != null ? src.ClientLocation.Npi : string.Empty))
                .ReverseMap();

            CreateMap<ClaimStatusBatchClaim, GetClaimStatusBatchClaimsByBatchIdResponse>()
                .ForPath(x => x.CurrentLineItemStatusId, map => map.MapFrom(src => src.ClaimStatusTransaction != null ? src.ClaimStatusTransaction.ClaimLineItemStatusId : null))
                .ForPath(x => x.EligibilityPolicyNumber, map => map.MapFrom(src => src.ClaimStatusTransaction != null ? src.ClaimStatusTransaction.EligibilityPolicyNumber : null))
                .ForPath(x => x.LastStatusChangedOn, map => map.MapFrom(src => src.ClaimStatusTransaction != null && src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ != null ? src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn : null))
                .ForPath(x => x.RenderingNpi, map => map.MapFrom(src => src.ClientProvider != null ? src.ClientProvider.Npi : src.RenderingNpi))
                .ForPath(x => x.PatientFirstName, map => map.MapFrom(src => src.Patient != null && src.Patient.Person != null ? src.Patient.Person.FirstName : string.Empty))
                .ForPath(x => x.PatientLastName, map => map.MapFrom(src => src.Patient != null && src.Patient.Person != null ?  src.Patient.Person.LastName : string.Empty))
                .ForPath(x => x.DateOfBirth, map => map.MapFrom(src => src.Patient != null && src.Patient.Person != null ? src.Patient.Person.DateOfBirth : new()))
                .ForPath(x => x.ClientLocationNpi, map => map.MapFrom(src => src.ClientLocation != null ? src.ClientLocation.Npi : string.Empty))
                .ReverseMap();

            CreateMap<ClaimStatusBatchClaim, GetQualifiedUnresolvedBatchClaimsByBatchIdResponse>()
                //.ForPath(x => x.CurrentLineItemStatusId, map => map.MapFrom(src => src.ClaimStatusTransaction != null ? src.ClaimStatusTransaction.ClaimLineItemStatusId : null))
                .ForPath(x => x.LastStatusChangedOn, map => map.MapFrom(src => src.ClaimStatusTransaction != null && src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ != null ? src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn : null))
                .ForPath(x => x.RenderingNpi, map => map.MapFrom(src => src.ClientProvider != null ? src.ClientProvider.Npi : src.RenderingNpi))
                .ForPath(x => x.PatientFirstName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.FirstName : string.Empty : string.Empty))
                .ForPath(x => x.PatientLastName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.LastName : string.Empty : string.Empty))
                .ForPath(x => x.DateOfBirth, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.DateOfBirth : new() : new()))
                .ForPath(x => x.ClientLocationNpi, map => map.MapFrom(src => src.ClientLocation != null ? src.ClientLocation.Npi : string.Empty))
                .ReverseMap();

            CreateMap<ClaimStatusBatchClaim, GetClaimStatusBatchClaimByIdResponse>()
                //.ForPath(x => x.CurrentLineItemStatusId, map => map.MapFrom(src => src.ClaimStatusTransaction != null ? src.ClaimStatusTransaction.ClaimLineItemStatusId : null))
                .ForPath(x => x.LastStatusChangedOn, map => map.MapFrom(src => src.ClaimStatusTransaction != null && src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ != null ? src.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn : null))
                .ForPath(x => x.RenderingNpi, map => map.MapFrom(src => src.ClientProvider != null ? src.ClientProvider.Npi : src.RenderingNpi))
                .ForPath(x => x.PatientFirstName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.FirstName : string.Empty : string.Empty))
                .ForPath(x => x.PatientLastName, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.LastName : string.Empty : string.Empty))
                .ForPath(x => x.DateOfBirth, map => map.MapFrom(src => src.Patient != null ? src.Patient.Person != null ? src.Patient.Person.DateOfBirth : new() : new()))
                .ForPath(x => x.ClientLocationNpi, map => map.MapFrom(src => src.ClientLocation != null ? src.ClientLocation.Npi : string.Empty))
                .ReverseMap();

            //CreateMap<ClaimStatusBatchClaimModel, ClaimStatusBatchClaim>().ReverseMap(); //AA-171
            //CreateMap<ICollection<ClaimStatusBatchClaimModel>, ICollection<ClaimStatusBatchClaim>>().ReverseMap();
            //CreateMap<ClaimsDataModel, ClaimStatusBatchClaim>()
            //    .ForPath(x => x.DateOfBirthString, map => map.MapFrom(y => y.PATIENT_BD))
            //    .ForPath(x => x.BilledAmountString, map => map.MapFrom(y => y.BILLED_AMT.ToString()))
            //    .ForPath(x => x.DateOfBirth, map => map.MapFrom(y => y.PATIENT_BD))
            //    .ForPath(x => x.DateOfServiceFromString, map => map.MapFrom(y => y.DOS_FROM))
            //    .ForPath(x => x.DateOfServiceToString, map => map.MapFrom(y => y.DOS_TO))
            //    .ForPath(x => x.ClaimBilledOnString, map => map.MapFrom(y => y.BILLED_DATE))
            //    .ForPath(x => x.BilledAmount, map => map.MapFrom(y => y.BILLED_AMT))
            //    //.ForPath(x => x.PatientFirstName, map => map.MapFrom(y => y.PATIENT_NM.Substring(0, y.PATIENT_NM.IndexOf(",")).Trim()))
            //    //.ForPath(x => x.PatientLastName, map => map.MapFrom(y => y.PATIENT_NM.Substring(0, y.PATIENT_NM.IndexOf(",") + 1).Trim()))
            //    .ForPath(x => x.PatientFirstName, map => map.MapFrom(y => y.PATIENT_NM))
            //    .ForPath(x => x.PatientLastName, map => map.MapFrom(y => y.PATIENT_NM))
            //    .ForPath(x => x.PolicyNumber, map => map.MapFrom(y => y.PATIENT_MEDICAID_ID))
            //    .ForPath(x => x.Quantity, map => map.MapFrom(y => y.QUANTITY))
            //    .ForPath(x => x.ProcedureCode, map => map.MapFrom(y => y.PROC_CODE)).ReverseMap();
            //CreateMap<ICollection<ClaimsDataModel>, ICollection<ClaimStatusBatchClaim>>().ReverseMap();
        }
    }
}
