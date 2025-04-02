using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class EmployeeClientRepository : RepositoryAsync<EmployeeClient, int>, IEmployeeClientRepository
    {
        private readonly ApplicationContext _dbContext;
        public EmployeeClientRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext; 
        }

        //public IQueryable<EmployeeClient> Entities => _dbContext.Set<EmployeeClient>();

        public async Task<List<EmployeeClient>> GetByEmployeeId(int employeeId)
        {
            return await _dbContext.EmployeeClients
                   .Where(x => x.EmployeeId == employeeId)
                   .Include(z => z.Client)
                   .ToListAsync();
        }
    }
}
