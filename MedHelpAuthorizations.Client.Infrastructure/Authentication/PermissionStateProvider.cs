using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Authentication
{
    public class PermissionStateProvider
    {
        public Dictionary<string, List<string>> RolesPermissions { get; set; }
        public PermissionStateProvider()
        {
            RolesPermissions = new Dictionary<string, List<string>>();

        }
        public void SetRoles(Dictionary<string, List<string>> rolesPermissions)
        {
            RolesPermissions = rolesPermissions;
        }
    }
}
