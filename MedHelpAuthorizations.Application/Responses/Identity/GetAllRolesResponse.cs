using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}