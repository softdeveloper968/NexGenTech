using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ClientFeeScheduleEntryByClientIdSpecification : HeroSpecification<ClientFeeScheduleEntry>
	{
		public ClientFeeScheduleEntryByClientIdSpecification(int clientId)
		{
			Criteria = p => p.ClientId == clientId;
		}
	}
}
