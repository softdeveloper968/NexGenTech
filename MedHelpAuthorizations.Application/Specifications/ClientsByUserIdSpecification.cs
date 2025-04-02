using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientsByUserIdSpecification : HeroSpecification<UserClient>
    {
        public ClientsByUserIdSpecification(string userId)
        {
            Includes.Add(x => x.Client);

            Criteria = p => p.UserId == userId;

        }
    }
}
