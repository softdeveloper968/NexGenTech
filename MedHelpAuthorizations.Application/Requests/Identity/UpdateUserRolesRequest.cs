using MedHelpAuthorizations.Application.Responses.Identity;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class UpdateUserRolesRequest
    {
        public IList<string> UserRoles { get; set; }
    }
}