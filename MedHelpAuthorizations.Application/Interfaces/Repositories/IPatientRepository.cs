
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IPatientRepository : IRepositoryAsync<Patient, int>
    {
        IQueryable<Patient> Patients { get; }

        Task<int> InsertAsync(Patient Patient);

        Task<Person> GetPersonByInputInfo(int clientId, string firstName, string LastName, DateTime? DOB);

        Task<Patient> GetpatientByInputInfo(int Personid);
        Task<List<PatientLedgerPayment>> GetPatientLedgerPaymentsAsync();
    }
}
