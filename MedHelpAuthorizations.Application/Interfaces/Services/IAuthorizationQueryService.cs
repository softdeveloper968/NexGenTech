using MedHelpAuthorizations.Application.Interfaces.Common;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IAuthorizationQueryService : IService
    {

        //Active Authorizations
        Task<int> GetAllActiveAuthorizationsCountAsync();
        Task<int> GetActiveSudAuthorizationsCountAsync();
        Task<int> GetActiveOmhcAuthorizationsCountAsync();
        Task<int> GetActivePrpAuthorizationsCountAsync();
        Task<int> GetActiveMhAuthorizationsCountAsync();
        Task<int> GetActiveOtherAuthorizationsCountAsync();

        //Authorizations Not Completed
        Task<int> GetAllAuthorizationsNotCompletedCountAsync();
        Task<int> GetSudAuthorizationsNotCompletedCountAsync();
        Task<int> GetOmhcAuthorizationsNotCompletedCountAsync();
        Task<int> GetPrpAuthorizationsNotCompletedCountAsync();
        Task<int> GetMhAuthorizationsNotCompletedCountAsync();
        Task<int> GetOtherAuthorizationsNotCompletedCountAsync();



        //Authorizations Discharged This Month
        Task<int> GetAllAuthorizationsDischargedMtdCountAsync();
        Task<int> GetSudAuthorizationsDischargedMtdCountAsync();
        Task<int> GetOmhcAuthorizationsDischargedMtdCountAsync();
        Task<int> GetPrpAuthorizationsDischargedMtdCountAsync();
        Task<int> GetMhAuthorizationsDischargedMtdCountAsync();
        Task<int> GetOtherAuthorizationsDischargedMtdCountAsync();

    }
}