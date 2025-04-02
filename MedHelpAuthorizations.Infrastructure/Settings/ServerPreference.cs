using MedHelpAuthorizations.Shared.Settings;

namespace MedHelpAuthorizations.Infrastructure.Settings
{
    public record ServerPreference : IPreference
    {
        //private readonly ICurrentUserService _currentUserService;

        public ServerPreference(
            //ICurrentUserService currentUserService
            )
        {
            //_currentUserService = currentUserService;
        }

        //TODO - add server preferences
    }
}
