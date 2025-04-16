using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.IdentityEntities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.IdentityEntities
{
    public class Server : AuditableEntity<int>
    {
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public ServerType ServerType { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
    }
}
