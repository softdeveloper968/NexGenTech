using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
    public class UserRolesResponse
    {
        public List<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
    }

    public class UserAllRolesResponse
    {
        public List<RoleNamesResponse> Roles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }

    public class UserRoleModel
    {
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}