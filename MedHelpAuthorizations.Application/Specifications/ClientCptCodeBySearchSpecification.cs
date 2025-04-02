using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ClientCptCodeBySearchSpecification : HeroSpecification<ClientCptCode>
	{
		public ClientCptCodeBySearchSpecification(string search, int clientId)
		{
			Criteria = p => p.ClientId == clientId;

			if (!string.IsNullOrEmpty(search))
			{
				// Normalize search term to lowercase for case-insensitive comparison
				var searchLower = search.ToLower();

				Criteria = p => p.ClientId == clientId &&
							(p.Code.ToLower().Contains(searchLower) ||
							 p.LookupName.ToLower().Contains(searchLower) ||
							 p.Description.ToLower().Contains(searchLower));
			}
		}
	}
}
