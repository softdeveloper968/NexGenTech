namespace MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Queries.GetAllByClientId
{
    public class GetClientApiKeyByClientIdResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ApiIntegrationId { get; set; }
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
    }
}
