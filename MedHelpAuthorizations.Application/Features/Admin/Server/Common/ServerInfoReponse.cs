using MedHelpAuthorizations.Application.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Server.Common
{
    public class ServerInfoReponse: AuditableResponse
    {
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
        public int ServerType { get; set; }
        public int AuthenticationType { get; set; }
        public string Username { get; set; }

    }
}
