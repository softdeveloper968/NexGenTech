using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IX12ClaimCodeLineItemStatusRepository : IRepositoryAsync<X12ClaimCodeLineItemStatus, int>
	{
        IQueryable<X12ClaimCodeLineItemStatus> X12ClaimCodeLineItemStatuses { get; }

        Task<List<X12ClaimCodeLineItemStatus>> GetAllForClaimLineItemStatusId(ClaimLineItemStatusEnum claimLineItemStatusID);

		Task<X12ClaimCodeLineItemStatus> GetByX12ClaimCodeString(string x12ClaimCode);

		Task<List<X12ClaimCodeLineItemStatus>> GetListByX12DelimitedClaimCodeString(string commaDelimitedClaimCodes);

	}
}
