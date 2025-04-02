using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Common;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IClaimStatusEmailService : IService
    {
        Task GetClaimCategoryCountsByBatchIdAndSendEmail(int batchId, string rpaCode, string batchCreatedDate);
    }
}
