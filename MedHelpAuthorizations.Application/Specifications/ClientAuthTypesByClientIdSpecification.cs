using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientAuthTypesByClientIdSpecification : HeroSpecification<ClientAuthType>
    {
        public ClientAuthTypesByClientIdSpecification(int clientId)
        {
            Includes.Add(x => x.AuthType);
            Criteria = p => p.ClientId == clientId;
        }
    }
}
