using MedHelpAuthorizations.Application.Interfaces.Common;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IPatientQueryService : IService
    {
        Task<int> GetPatientsAddedMtdCountAsync();
        Task<int> GetPatientsAddedMtdNoBenefitCheckCountAsync();
        Task<int> GetActivePatientsCountAsync();
        
    }
}