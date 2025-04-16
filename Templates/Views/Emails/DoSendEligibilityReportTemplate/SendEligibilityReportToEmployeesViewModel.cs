namespace MedHelpAuthorizations.Template.Views.Emails.DoSendEligibilityReportTemplate
{
    public class SendEligibilityReportToEmployeesViewModel
    {
        public string FirstName { get; set; }
        public string Greeting { get; set; }
        public List<InsuranceItemViewModel> CorrectedInsurances { get; set; }
        public List<InsuranceItemViewModel> EligibleInsurances { get; set; }
        public List<InsuranceItemViewModel> DiscoveredEligibility { get; set; }
        public List<InsuranceItemViewModel> SelfPayEligibility { get; set; }
        public string ReviewedInsurancesTotalCount { get; set; }
    }

    public class InsuranceItemViewModel
    {
        public string PayerName { get; set; }
        public int Count { get; set; }
    }
}
