namespace MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit
{
    public class ApiKeyViewModel
    {
        public int Id { get; set; }
        public int ApiIntegrationId { get; set; }
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public int ClientId { get; set; }
    }
}
