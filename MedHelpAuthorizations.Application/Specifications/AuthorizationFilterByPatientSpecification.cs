using MedHelpAuthorizations.Application.Specifications.Base;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class AuthorizationFilterByPatientSpecification : HeroSpecification<Authorization>
    {
        public AuthorizationFilterByPatientSpecification(int patientId)
        {
            Includes.Add(a => a.Documents);
            Criteria = p => p.PatientId == patientId;            
        }
    }
}