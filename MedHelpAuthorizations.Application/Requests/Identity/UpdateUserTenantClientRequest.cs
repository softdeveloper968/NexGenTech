using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class UpdateUserTenantClientRequest
    {
        public Dictionary<int, List<int>> TenantClients { get; set; }
    }
    public class UpdateUserClientRequest
    {
        public int TenantId { get; set; }
        public List<int> Clients { get; set; }
    }
}
