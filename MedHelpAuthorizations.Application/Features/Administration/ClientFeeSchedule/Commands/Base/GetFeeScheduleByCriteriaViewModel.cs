using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base
{
	public class GetFeeScheduleByCriteriaViewModel
	{
		public int Id { get; set; }
		public int ClientInsuranceId { get; set; }
		public DateTime ReferencedDateOfServiceFrom { get; set; }
        public int DateOfServiceYear { get; set; }
		public int ClientId { get; set; }
		public string ProcedureCode { get; set; }
		public decimal BilledAmount { get; set; }
		public decimal? LastReimbursement {  get; set; }
		public ClientCptCodeDto ClientCptCode { get; set; }

    }
}
