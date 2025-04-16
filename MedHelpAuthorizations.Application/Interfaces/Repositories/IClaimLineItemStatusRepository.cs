using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClaimLineItemStatusRepository
    {
        Task<List<ClaimLineItemStatus>> GetListAsync();

        Task<ClaimLineItemStatus> GetByIdAsync(ClaimLineItemStatusEnum id);
    }
}
