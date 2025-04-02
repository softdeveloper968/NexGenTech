using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetBase;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetById
{
    public class GetClientInsuranceRpaConfigurationByIdResponse
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ClientInsuranceId { get; set; }

        public int RpaInsuranceId { get; set; }

        public string ExternalId { get; set; }

        public TransactionTypeEnum TransactionTypeId { get; set; }

        public int AuthTypeId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string TargetUrl { get; set; }

        public bool FailureReported { get; set; }

        public string FailureMessage { get; set; }

        public string ReportFailureToEmail { get; set; }

        public bool IsDeleted { get; set; }
    }
}
