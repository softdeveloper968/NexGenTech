using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetCleanupBatches;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByFailedConfigurations.Queries;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Base;
using System.Linq;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClaimStatusBatchProfile : Profile
    {
        public ClaimStatusBatchProfile()
        {
            CreateMap<CreateClaimStatusBatchCommand, ClaimStatusBatch>().ReverseMap();
            CreateMap<UpdateCompletedClaimStatusBatchCommand, ClaimStatusBatch>().ReverseMap();
            CreateMap<GetClaimStatusBatchByIdResponse, ClaimStatusBatch>();
            //CreateMap<GetClaimStatusUnprocessedBatchesResponse, ClaimStatusBatch>().ReverseMap();
            CreateMap<GetAllClaimStatusBatchesResponse, ClaimStatusBatch>().ReverseMap();
            //CreateMap<GetRecentClaimStatusBatchesByClientIdResponse, ClaimStatusBatch>().ReverseMap();
			CreateMap<ClaimStatusBatch, GetAllCaimStatusBatchClaimResponse>().ReverseMap(); //AA-231
            CreateMap<ClaimStatusBatch, GetRecentClaimStatusBatchesByClientIdResponse>()
                .ForPath(x => x.RpaInsuranceCode, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null ? y.ClientInsurance.RpaInsurance.Code : string.Empty))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsuranceId))
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.Client.Id))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
                .ForPath(x => x.ClientInsuranceLookupName, map => map.MapFrom(y => y.ClientInsurance != null ? y.ClientInsurance.LookupName : string.Empty))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType != null ? y.AuthType.Name : string.Empty))
                .ForPath(x => x.ClientLocationId, map => map.MapFrom(y => y.InputDocument != null && y.InputDocument.ClientLocation != null ? y.InputDocument.ClientLocation.Id : (int?)null))
                .ForPath(x => x.ClientLocationInsuranceIdentifier, map => map.MapFrom(y => y.InputDocument != null && y.InputDocument.ClientLocation != null && y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers != null
                                                                                                            ? y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers.Select(x => x.Id).ToList()
                                                                                                            : new List<int>()));
            //.ForPath(x => x.BatchAssignmentCount, map => map.MapFrom(y => y.ClaimStatusBatchHistories.Where(bh => bh.CreatedOn > DateTime.UtcNow.AddDays(-14)).DistinctBy(bh => bh.AssignedDateTimeUtc).Count()));
            //    map => map.MapFrom(y => y.ClaimStatusBatchHistories
            //                .Where(bh => bh.CreatedOn > DateTime.UtcNow.AddDays(-14))
            //                .DistinctBy(bh => bh.AssignedDateTimeUtc)
            //                .Select(b => (DateTime)b.AssignedDateTimeUtc)
            //                .OrderByDescending(ad => ad)
            //                .ToList()));
            CreateMap<ClaimStatusBatch, GetClaimStatusBatchByIdResponse>()
                .ForPath(x => x.RpaInsuranceCode, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Code))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Id))
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.Client.Id))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
                .ForPath(x => x.ClientInsuranceLookupName, map => map.MapFrom(y => y.ClientInsurance.LookupName))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType.Name))
                .ForPath(x => x.ClientLocationId, map => map.MapFrom(y => y.InputDocument.ClientLocation != null ? y.InputDocument.ClientLocation.Id : (int?)null))
                .ForPath(x => x.ClientLocationInsuranceIdentifier, map => map.MapFrom(y => y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers != null
                                                                                                            ? y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers.Select(x => x.Id).ToList()
                                                                                                            : new List<int>()));
            //.ForPath(x => x.BatchAssignmentCount, map => map.MapFrom(y => y.ClaimStatusBatchHistories.Where(bh => bh.CreatedOn > DateTime.UtcNow.AddDays(-14)).DistinctBy(bh => bh.AssignedDateTimeUtc).Count()));

            CreateMap<ClaimStatusBatch, GetClaimStatusUnprocessedBatchesResponse>()
                .ForPath(x => x.RpaInsuranceCode, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Code))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Id))
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.Client.Id))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
                .ForPath(x => x.ClientInsuranceLookupName, map => map.MapFrom(y => y.ClientInsurance.LookupName))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType.Name))
                .ForPath(x => x.RpaGroupName, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.RpaInsuranceGroup != null ? y.ClientInsurance.RpaInsurance.RpaInsuranceGroup.Name : "RpaGroup Name Missing")) //AA-316
                .ForPath(x => x.ClientLocationId, map => map.MapFrom(y => y.InputDocument.ClientLocation != null ? y.InputDocument.ClientLocation.Id : (int?)null))
                .ForPath(x => x.ClientLocationInsuranceIdentifier, map => map.MapFrom(y => y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers != null
                                                                                                            ? y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers.Select(x => x.Id).ToList()
                                                                                                            : new List<int>()));
            //.ForPath(x => x.BatchAssignmentCount, map => map.MapFrom(y => y.ClaimStatusBatchHistories.Where(bh => bh.CreatedOn > DateTime.UtcNow.AddDays(-14)).DistinctBy(bh => bh.AssignedDateTimeUtc).Count()));

            CreateMap<ClaimStatusBatch, GetAllClaimStatusBatchesResponse>()
                .ForPath(x => x.RpaInsuranceCode, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Code))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Id))
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.Client.Id))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
                .ForPath(x => x.ClientInsuranceLookupName, map => map.MapFrom(y => y.ClientInsurance.LookupName))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType.Name))
                .ForPath(x => x.ClientLocationId, map => map.MapFrom(y => y.InputDocument.ClientLocation != null ? y.InputDocument.ClientLocation.Id : (int?)null))
                .ForPath(x => x.ClientLocationInsuranceIdentifier, map => map.MapFrom(y => y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers != null
                                                                                                            ? y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers.Select(x => x.Id).ToList()
                                                                                                            : new List<int>()));
            //.ForPath(x => x.BatchAssignmentCount, map => map.MapFrom(y => y.ClaimStatusBatchHistories.Where(bh => bh.CreatedOn > DateTime.UtcNow.AddDays(-14)).DistinctBy(bh => bh.AssignedDateTimeUtc).Count()));

            CreateMap<ClaimStatusBatch, GetCompletedCleanupByHostnameResponse>()
                .ForPath(x => x.RpaInsuranceCode, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Code))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance.Id))
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.Client.Id))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
                .ForPath(x => x.ClientInsuranceLookupName, map => map.MapFrom(y => y.ClientInsurance.LookupName))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType.Name))
                .ForPath(x => x.ClientLocationId, map => map.MapFrom(y => y.InputDocument.ClientLocation != null ? y.InputDocument.ClientLocation.Id : (int?)null))
                .ForPath(x => x.ClientLocationInsuranceIdentifier, map => map.MapFrom(y => y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers != null
                                                                                                            ? y.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers.Select(x => x.Id).ToList()
                                                                                                            : new List<int>()));
            //.ForPath(x => x.BatchAssignmentCount, map => map.MapFrom(y => y.ClaimStatusBatchHistories.Where(bh => bh.CreatedOn > DateTime.UtcNow.AddDays(-14)).DistinctBy(bh => bh.AssignedDateTimeUtc).Count()));

            CreateMap<GetBatchesByFailedConfigurationsResponse, ClaimStatusBatch>()
              .ForPath(x => x.ClientInsurance.RpaInsurance.Code, map => map.MapFrom(y => y.RpaInsuranceCode))
              .ForPath(x => x.ClientInsurance.RpaInsurance.Id, map => map.MapFrom(y => y.RpaInsuranceId))
              .ForPath(x => x.Client.Id, map => map.MapFrom(y => y.ClientId))
              .ForPath(x => x.Client.ClientCode, map => map.MapFrom(y => y.ClientCode))
              .ForPath(x => x.ClientInsurance.LookupName, map => map.MapFrom(y => y.ClientInsuranceLookupName))
              .ForPath(x => x.AuthType.Name, map => map.MapFrom(y => y.AuthTypeName))
              .ForPath(x => x.AbortedReason, map => map.MapFrom(y => y.AbortedReason))
              .ForPath(x => x.InputDocument.ClientLocationId, map => map.MapFrom(y => y.ClientLocationId))
              .ForPath(x => x.InputDocument.ClientLocation.ClientLocationInsuranceIdentifiers, map => map.MapFrom(y => y.ClientLocationInsuranceIdentifier));
        }
    }
}
