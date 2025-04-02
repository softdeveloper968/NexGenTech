using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
	internal class ClientKpiByClientIdSpecification : HeroSpecification<ClientKpi>
	{
		public ClientKpiByClientIdSpecification(int clientId)
		{
			Criteria = p => p.ClientId == clientId;
		}
	}
}
