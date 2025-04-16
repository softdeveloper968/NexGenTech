using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetByClientId
{
    public class GetClientAuthTypesByClientIdResponse
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public GetAllPagedAuthTypesResponse AuthType { get; set; }
    }
}
