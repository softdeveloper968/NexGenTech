using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientFeeScheduleRepository : IClientFeeScheduleRepository
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IRepositoryAsync<Domain.Entities.ClientFeeSchedule, int> _repository;

        public ClientFeeScheduleRepository(
            IUnitOfWork<int> unitOfWork,
            IRepositoryAsync<Domain.Entities.ClientFeeSchedule, int> repository
            )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ClientFeeSchedule> GetByIdAsync(int Id)
        {
            return await _repository.Entities
                               .Include(x => x.ClientInsuranceFeeSchedules)
                               .Include(x => x.ClientFeeScheduleSpecialties)
                               .Include(x => x.ClientFeeScheduleProviderLevels)
                               .FirstOrDefaultAsync(x => x.Id == Id);

        }
    }
}
