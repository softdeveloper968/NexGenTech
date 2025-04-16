namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Command.AddEdit
{
    public class AddEditClientInsuranceFeeScheduleViewModel
    {
        public int Id { get; set; }
        public int ClientFeeScheduleId { get; set; }
        public int ClientInsuranceId { get; set; }
        public bool IsActive { get; set; }
    }
}
