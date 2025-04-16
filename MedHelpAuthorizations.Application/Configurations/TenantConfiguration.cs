using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Configurations
{
    public class TenantConfiguration
    {
        public string Identifier { get; set; }
        public string TenantName { get; set; }
        public string EmailAddress { get; set; }
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public int DBAutheticationType { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
