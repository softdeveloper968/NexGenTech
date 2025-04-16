using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class RpaTypeRepository : RepositoryAsync<RpaType, RpaTypeEnum>, IRpaTypeRepository
    {
        private readonly ApplicationContext _dbContext;

        public RpaTypeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<RpaType> RpaTypes { get; }


        //public async Task<RpaType> GetByIdAsync(int id)
        //{
        //    return await _dbContext.RpaTypes
        //        .Where(r => r.Id == (RpaTypeEnum)id)
        //        .FirstOrDefaultAsync();
        //}
    }
}
