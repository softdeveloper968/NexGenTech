namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base
{
	public class FeeScheduleCriteriaResultViewModel
	{
		public decimal AverageLineItemPaidAmount { get; set; }
		public string ProcedureCode { get; set; }
		public decimal AverageBilledAmount { get; set; }
		public decimal ChargedSum { get; set; }
		public int ClientCptCodeId { get; set; }
	}
}
