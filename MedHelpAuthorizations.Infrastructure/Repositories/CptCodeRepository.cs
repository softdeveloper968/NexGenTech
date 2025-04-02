using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class CptCodeRepository : RepositoryAsync<CptCode, int>, ICptCodeRepository
    {
        private readonly ApplicationContext _dbContext;
        public CptCodeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CptCode> GetByCptCode(string code)
        {
            code = code.ToUpper().Trim();
            return await _dbContext.CptCodes
                .FirstOrDefaultAsync(c => c.Code.ToUpper().Trim() == code);
        }
    }
}
