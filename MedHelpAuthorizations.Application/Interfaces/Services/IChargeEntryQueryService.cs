using MedHelpAuthorizations.Application.Interfaces.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IChargeEntryQueryService : IService
    {
        Task<List<ChargeEntryRpaConfiguration>> GetUiPathChargeEntryConfigurations();
    }
}
