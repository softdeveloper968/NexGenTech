namespace MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports
{
    public class ClaimStatusTotalResultExceptionModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
