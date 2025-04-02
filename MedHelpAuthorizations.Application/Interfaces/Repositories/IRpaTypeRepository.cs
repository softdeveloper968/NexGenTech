using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IRpaTypeRepository : IRepositoryAsync<RpaType, RpaTypeEnum>
    {
        IQueryable<RpaType> RpaTypes { get; }
    }
}
