using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class PermissionRequest
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsRequest> RoleClaims { get; set; }
    }
}