
namespace MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Base
{
    public class ClientAuthTypeDto
    {
        public int AuthTypeId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; } = default(string);
        public string Description { get; set; } = default(string);
    }
}
