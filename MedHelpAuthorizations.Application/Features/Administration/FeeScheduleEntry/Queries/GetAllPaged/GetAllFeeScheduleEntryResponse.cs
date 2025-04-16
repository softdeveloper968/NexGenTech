using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged
{
    public class GetAllFeeScheduleEntryResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ClientCptCodeId { get; set; }
        public int ClientFeeScheduleId { get; set; }
        public decimal Fee { get; set; }
        public decimal AllowedAmount { get; set; }
        public decimal ReimbursablePercentage { get; set; } //EN-70
		public bool IsReimbursable { get; set; }
        public ClientCptCodeDto ClientCptCode { get; set; }
        public ImportStatusEnum importStatus { get; set; }
    }
}
