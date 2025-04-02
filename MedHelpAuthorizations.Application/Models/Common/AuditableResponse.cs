using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Models.Common
{
    public class AuditableResponse
    {
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedByName { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
