using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    internal class InsuranceCardsByPatientIdFilterSpecification : HeroSpecification<InsuranceCard>
    {
        public InsuranceCardsByPatientIdFilterSpecification(int patientId, int clientId)
        {
            Includes.Add(x => x.Cardholder);
            Includes.Add(x => x.Cardholder.Person);
            Includes.Add(x => x.Insurance);
            Includes.Add(x => x.Patient);
            Includes.Add(x => x.Patient.Person);
            Criteria = p => p.ClientId == clientId && p.PatientId == patientId;
        }
    }
}
