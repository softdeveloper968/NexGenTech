using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
	public interface IFeeScheduleEntryRepository
	{
		public Task<ClientFeeScheduleEntry> GetById(int id);

	}
}
