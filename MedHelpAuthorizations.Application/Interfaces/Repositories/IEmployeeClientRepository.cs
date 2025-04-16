using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IEmployeeClientRepository : IRepositoryAsync<EmployeeClient, int>
    {
        public Task<List<EmployeeClient>> GetByEmployeeId(int employeeId);
    }
}
