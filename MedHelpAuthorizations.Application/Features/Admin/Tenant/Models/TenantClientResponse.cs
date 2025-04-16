using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Models
{
    public class BasicClientReponse
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
    public class BasicTenantClientResponse
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public List<BasicClientReponse> Clients { get; set; } = new List<BasicClientReponse>();
        public List<BasicClientReponse> ClientEmployees { get; set; } = new List<BasicClientReponse>();
    }
}
