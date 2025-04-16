using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ClientFeeScheduleEntryByClientFeeScheduleIdSpecification : HeroSpecification<ClientFeeScheduleEntry>
	{
		public ClientFeeScheduleEntryByClientFeeScheduleIdSpecification(int clientFeeScheduleId)
		{
			Criteria = p => p.ClientFeeScheduleId == clientFeeScheduleId;
		}
	}
}
