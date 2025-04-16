using System.Linq;


namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    using MedHelpAuthorizations.Domain.IntegratedServices;

    public interface IClaimStatusExceptionReasonCategoryMapRepository 
    {
        IQueryable<ClaimStatusExceptionReasonCategoryMap> ClaimStatusExceptionReasonCategoryMaps { get; }

        Task<ClaimStatusExceptionReasonCategoryMap> GetByExceptionCategoryReasonAsync(string claimStatusExceptionCategoryReason);
    }
}
