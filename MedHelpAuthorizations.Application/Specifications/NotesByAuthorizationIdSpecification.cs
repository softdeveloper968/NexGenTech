using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class NotesByAuthorizationIdSpecification : HeroSpecification<Note>
    {
        public NotesByAuthorizationIdSpecification(int authid)
        {
            Criteria = p => p.AuthorizationId == authid;
        }
    }
}
