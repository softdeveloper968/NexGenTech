using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IX12ClaimCategoryCodeLineItemStatusRepository : IRepositoryAsync<X12ClaimCategoryCodeLineItemStatus, int>
	{
        IQueryable<X12ClaimCategoryCodeLineItemStatus> X12ClaimCategoryCodeLineItemStatuses { get; }

        Task<List<X12ClaimCategoryCodeLineItemStatus>> GetAllForClaimLineItemStatusId(ClaimLineItemStatusEnum claimLineItemStatusID);

		Task<X12ClaimCategoryCodeLineItemStatus> GetByX12ClaimCategoryCodeString(string x12ClaimCategoryCode); 
    }
}
