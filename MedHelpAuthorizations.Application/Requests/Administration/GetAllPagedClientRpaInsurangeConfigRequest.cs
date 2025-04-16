using MedHelpAuthorizations.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedClientRpaInsurangeConfigRequest : PagedRequest
    {
        public string SearchString { get; set; } = string.Empty;
    }
}
