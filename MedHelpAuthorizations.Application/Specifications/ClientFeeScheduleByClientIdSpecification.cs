using MedHelpAuthorizations.Application.Specifications.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
    internal class ClientFeeScheduleByClientIdSpecification : HeroSpecification<Domain.Entities.ClientFeeSchedule>
    {
        public ClientFeeScheduleByClientIdSpecification(int clientId)
        {
            Criteria = p => p.ClientId == clientId;
        }
    }
}
