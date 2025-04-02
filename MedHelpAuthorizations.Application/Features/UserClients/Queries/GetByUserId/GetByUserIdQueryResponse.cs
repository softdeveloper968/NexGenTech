namespace MedHelpAuthorizations.Application.Features.UserClients.Queries.GetByUserId
{
    public class GetByUserIdQueryResponse 
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
    }
}
