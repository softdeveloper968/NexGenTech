using MedHelpAuthorizations.Application.Extensions;using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;using MedHelpAuthorizations.Application.Helpers;using MedHelpAuthorizations.Application.Specifications.Base;using MedHelpAuthorizations.Domain.Entities.IntegratedServices;namespace MedHelpAuthorizations.Application.Specifications{    public class ClaimStatusBatchClaimTrendsFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>    {        public ClaimStatusBatchClaimTrendsFilterSpecification(IClaimStatusDashboardQueryBase query, int clientId)        {            Criteria = c => true;            Criteria = Criteria.And(c => !c.IsDeleted);            Criteria = Criteria.And(c => c.ClientId == clientId);



            //if (query.ClientInsuranceId > 0)
            //{
            //    Criteria = Criteria.And(c => c.ClaimStatusBatch.ClientInsuranceId == query.ClientInsuranceId);
            //}

            //if (query.AuthTypeId > 0)
            //{
            //    Criteria = Criteria.And(c => c.ClaimStatusBatch.AuthTypeId == query.AuthTypeId);
            //}

            //if (!string.IsNullOrWhiteSpace(query.ProcedureCode))
            //{
            //    Criteria = Criteria.And(c => c.ProcedureCode.Trim() == query.ProcedureCode.Trim());
            //}

            ///AA-120                                  if (!string.IsNullOrEmpty(query.ClientInsuranceIds))            {                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.ClientInsuranceIds, true).Contains(c.ClaimStatusBatch.ClientInsuranceId));            }            if (!string.IsNullOrEmpty(query.AuthTypeIds))            {                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.AuthTypeIds, true).Contains(c.ClaimStatusBatch.AuthTypeId ?? 0));            }            if (!string.IsNullOrWhiteSpace(query.ProcedureCodes))            {                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertProcedureCodesToList(query.ProcedureCodes).Contains(c.ProcedureCode.Trim()));            }        }    }}