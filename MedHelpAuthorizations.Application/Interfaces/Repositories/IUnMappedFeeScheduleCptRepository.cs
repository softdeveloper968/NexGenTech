using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
	public interface IUnMappedFeeScheduleCptRepository
	{
		Task<List<UnmappedFeeScheduleCpt>> GetByClientId(int clientId);
		Task<UnmappedFeeScheduleCpt> GetByCriteria(int clientCptCodeId, int clientInsuranceId, int dateOfService, int clientId);

    }
}
