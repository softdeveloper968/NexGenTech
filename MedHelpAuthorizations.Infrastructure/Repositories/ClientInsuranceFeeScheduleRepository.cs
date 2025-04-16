using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientInsuranceFeeScheduleRepository : IClientInsuranceFeeScheduleRepository
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IRepositoryAsync<ClientInsuranceFeeSchedule, int> _repository;

        public ClientInsuranceFeeScheduleRepository(
            IUnitOfWork<int> unitOfWork,
            IRepositoryAsync<ClientInsuranceFeeSchedule, int> repository
            )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public async Task<List<ClientInsuranceFeeSchedule>> GetClientInsuranceFeeScheduleByClientFeeScheduleId(int feeScheduleId)
        {
            return await _repository.Entities
               .Include(x => x.ClientInsurance)
               .Where(x => x.ClientFeeScheduleId == feeScheduleId)
               .ToListAsync();
        }

    }
}
