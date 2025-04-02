using AutoMapper;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using UhcClaimsApi.Responses.UHC.Claims.ByClaimNumber;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClaimStatusTransactionProfile : Profile
    {
        public ClaimStatusTransactionProfile()
        {
            CreateMap<UpsertClaimStatusTransactionCommand, ClaimStatusTransaction>()
                .ForMember(dest => dest.CreatedOn, src => { src.UseDestinationValue(); src.Ignore();})
                //.ForMember(dest => dest.ClaimStatusTransactionLineItemStatusChangẹId, src => src.Ignore())
                .ForMember(dest => dest.ClaimStatusExceptionReasonCategoryId, map => map.MapFrom(tr => tr.ExceptionReasonCategoryId))
                .ForMember(dest => dest.DateClaimFinalized, map => map.MapFrom(tr => tr.ClaimFinalizedOn))
                .ForMember(dest => dest.ClaimStatusTransactionLineItemStatusChangẹId, map => map.MapFrom(tr => tr.ClaimStatusTransactionLineItemStatusChangẹId));

            CreateMap<ClaimStatusTransaction, ClaimStatusDaysWaitLapsedDetailResponse>()
                .ForMember(dest => dest.ClaimStatusTransactionLineItemStatusChangẹId, map => map.MapFrom(tr => tr.ClaimStatusTransactionLineItemStatusChangẹId));

            CreateMap<GetClaimsDetailByClaimNumberResponse, UpsertClaimStatusTransactionCommand>();
				//.ForMember(dest => dest.CreatedOn, src => { src.UseDestinationValue(); src.Ignore(); })
				//.ForMember(dest => dest.ClaimStatusExceptionReasonCategoryId, map => map.MapFrom(src => src.))
				//.ForMember(dest => dest.CheckDate, map => map.MapFrom(src => src));

			CreateMap<GetClaimStatusTransactionByIdResponse, ClaimStatusTransaction>().ReverseMap();
            CreateMap<GetAllClaimStatusTransactionsResponse, ClaimStatusTransaction>().ReverseMap();
            CreateMap<AddEditClaimStatusWorkstationCommand, ClaimStatusTransaction>();
            CreateMap<AddEditClaimStatusMarkAsPaidCommand, AddEditClaimStatusWorkstationCommand>().ReverseMap();
            CreateMap<AddEditClaimStatusChangeStatusCommand, AddEditClaimStatusWorkstationCommand>().ReverseMap();
            CreateMap<ClaimStatusTransaction, AddEditClaimStatusWorkstationCommand>().ReverseMap();
            CreateMap<AddEditClaimStatusWorkstationCommand, ClaimWorkstationDetailsResponse>().ReverseMap();
            CreateMap<AddEditClaimStatusMarkAsPaidCommand, ClaimWorkstationDetailsResponse>().ReverseMap();
            CreateMap<AddEditClaimStatusChangeStatusCommand, ClaimWorkstationDetailsResponse>().ReverseMap();
            CreateMap<AddEditClaimStatusWorkstationNotesCommand, ClaimStatusWorkstationNotes>().ReverseMap();
            CreateMap<AddEditClaimStatusWorkstationNotesCommand, GetClaimStatusWorkstationNotesResponse>().ReverseMap();
            CreateMap<AddEditClaimStatusWorkstationNotesCommand, ClaimWorkstationDetailsResponse>().ReverseMap();

		//	// Claims Api Mappings to UpsertClaimStatusTransactionCommand
		//	CreateMap<Line, UpsertClaimStatusTransactionCommand>()
		//		//.ForMember(dest => dest.ClaimStatusBatchClaimId, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.LineItemControlNumber, map => map.MapFrom(src => src.))
		//		.ForMember(dest => dest.ClaimNumber, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ClaimFinalizedOn, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ClaimLineItemStatusId, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ClaimLineItemStatusValue, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ClaimStatusTransactionBeginDateTimeUtc, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ClaimStatusTransactionEndDateTimeUtc, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.LineItemChargeAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.DateEntered, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.DateReceived, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ReasonDescription, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ReasonDescription, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.RemarkCode, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.RemarkDescription, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.PlanType, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.TotalClaimStatusId, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.TotalClaimStatusValue, map => map.MapFrom(src => src.ClaimNumber))
		//		// Payment Properties
		//		.ForMember(dest => dest.CheckPaidAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.LineItemPaidAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.LineItemChargeAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.LineItemApprovedAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.TotalAllowedAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.TotalNonAllowedAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.CheckDate, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.DatePaid, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.CoinsuranceAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.CopayAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.DeductibleAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.TotalMemberResponsibilityAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.PaymentType, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.PenalityAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.CheckPaidAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.TotalClaimChargeAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.TotalClaimPaidAmount, map => map.MapFrom(src => src.ClaimNumber))
		//		//Denial Properties
		//		.ForMember(dest => dest.ExceptionReason, map => map.MapFrom(src => src.ClaimNumber))
		//		.ForMember(dest => dest.ExceptionRemark, map => map.MapFrom(src => src.ClaimNumber));

		}
	}
}
