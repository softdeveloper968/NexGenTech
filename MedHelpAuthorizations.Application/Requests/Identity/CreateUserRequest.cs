using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long? PhoneNumber { get; set; } = null;
        public bool IsActive { get; set; } = true;
        public bool AutoConfirmEmail { get; set; } = false;
        public List<string> Roles { get; set; }
        public List<int> Tenants { get; set; }
        public Dictionary<int,List<int>> TenantClients { get; set; }

    }
}
