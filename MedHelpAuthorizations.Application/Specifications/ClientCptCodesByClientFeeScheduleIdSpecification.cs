using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClientCptCodesByClientFeeScheduleIdSpecification : HeroSpecification<ClientFeeScheduleEntry>
	{
		public ClientCptCodesByClientFeeScheduleIdSpecification(string searchString, int ClientFeeScheduleId)
		{
			Criteria = p => p.ClientFeeScheduleId == ClientFeeScheduleId;

			if (!string.IsNullOrEmpty(searchString))
			{
				Criteria = Criteria.And(p => p.ClientCptCode.Code.Contains(searchString));
			}
		}
	}
}
