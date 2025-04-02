using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class PatientRepositoryAsync : RepositoryAsync<Patient, int>, IPatientRepository
    {
        private readonly ApplicationContext _dbContext;

        public PatientRepositoryAsync(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Patient> Patients => _dbContext.Patients;
             
        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return await Patients
                           .Include(x => x.Person)
                           .ThenInclude(y => y.Address)
                           .Where(p => p.Id == patientId)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> InsertAsync(Patient patient)
        {
            await _dbContext.AddAsync(patient);
            return patient.Id;
        }
        public async Task<Person> GetPersonByInputInfo(int clientId, string firstName, string LastName, DateTime? DOB)
        {
            return await _dbContext.Set<Person>().FirstOrDefaultAsync(x => x.FirstName == firstName && x.LastName == LastName && x.DateOfBirth == DOB && x.ClientId == clientId);
        }

        public async Task<Patient> GetpatientByInputInfo(int Personid)
        {
            return await _dbContext.Set<Patient>().FirstOrDefaultAsync(x => x.PersonId == Personid);
        }

        public async Task<List<PatientLedgerPayment>> GetPatientLedgerPaymentsAsync()
        {
            return await _dbContext.Set<PatientLedgerPayment>().Include(x=>x.PatientLedgerCharge).ToListAsync();
        }
    }
}
