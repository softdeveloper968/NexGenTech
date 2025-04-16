using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientLocationSpecialityRepository : IClientLocationSpecialityRepository
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IRepositoryAsync<ClientLocationSpeciality, int> _repository;
        public ClientLocationSpecialityRepository(
            IUnitOfWork<int> unitOfWork,
            IRepositoryAsync<ClientLocationSpeciality, int> repository
            )
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        /// <summary>
        /// Delete mappings from ClientLocationSpeciality table
        /// </summary>
        /// <param name="specialityLocationList"></param>
        /// <returns></returns>

        public async Task<bool> DeleteSpecialityLocationMappings(List<ClientLocationSpeciality> specialityLocationList, CancellationToken cancellationToken)
        {
            try
            {
                if (specialityLocationList.Any())
                {
                    foreach (var data in specialityLocationList)
                    {
                        await _repository.DeleteAsync(data);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// get all the Speciality-location mappings from ClientLocationSpeciality table by location Id
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public async Task<List<ClientLocationSpeciality>> GetLocationSpecialityMappingsByLocationId(int locationId)
        {
            return await _repository.Entities
                .Where(x => x.ClientLocationId == locationId)
                .ToListAsync();
        }

		public async Task<List<ClientLocationSpeciality>> GetClientLocationsSpecialityByLocationId(int clientId, int locationId)
		{
			return await _repository.Entities
					.Include(x => x.ClientLocation)
					//.Include(a => a.Specialty)
					.Where(x => x.ClientId == clientId && x.ClientLocationId == locationId)
					.ToListAsync();
		}
	}
}
