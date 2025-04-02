using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Responses.Identity
{
	public class TenantResponse
	{
        public int TenantId { get; set; }
        public string TenantIdentifier { get; set; }
        public string Name { get; set; }
    }
}
