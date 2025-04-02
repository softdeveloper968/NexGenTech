using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class RpaInsuranceGroup : AuditableEntity<int>
    {
        public string Name { get; set; }

        public string DefaultTargetUrl { get; set; }
    }
}
