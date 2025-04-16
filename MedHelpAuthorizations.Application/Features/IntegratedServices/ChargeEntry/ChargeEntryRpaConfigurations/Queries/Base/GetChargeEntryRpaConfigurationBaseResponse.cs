using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Base
{
    public class GetChargeEntryRpaConfigurationBaseResponse
    {        
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ChargeEntryId { get; set; }
        public RpaTypeEnum RpaTypeId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }
        public int AuthTypeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TargetUrl { get; set; }
        public bool FailureReported { get; set; }
        public bool IsDeleted { get; set; }
    }
}
