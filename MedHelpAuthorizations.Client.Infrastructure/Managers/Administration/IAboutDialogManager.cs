using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IAboutDialogManager : IManager
    {
        Task<IResult<ReleaseArtifactInfo>> GetPublishInformation();
    }
}
