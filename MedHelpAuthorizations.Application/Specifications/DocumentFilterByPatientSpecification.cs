using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class DocumentFilterByCriteriaSpecification : HeroSpecification<Document>
    {
        public DocumentFilterByCriteriaSpecification(int patientId, int? authorizationId = null)
        {
            Criteria = p => true;
            if (patientId > 0)
            {
                Criteria = Criteria.And(p => p.PatientId == patientId);
            }

            if (authorizationId != null && authorizationId > 0)
            {
                Criteria = Criteria.And(p => p.AuthorizationId == authorizationId);
            }
        }
    }    
}
