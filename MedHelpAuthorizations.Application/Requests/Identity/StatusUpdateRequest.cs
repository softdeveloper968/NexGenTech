namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class StatusUpdateRequest
    {
        public bool ActivateUser { get; set; }
        public string UserId { get; set; }
    }
}
