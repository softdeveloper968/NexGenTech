using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientInsuranceFeeScheduleRepository
    {
        Task<List<ClientInsuranceFeeSchedule>> GetClientInsuranceFeeScheduleByClientFeeScheduleId(int feeScheduleId);
    }
}
