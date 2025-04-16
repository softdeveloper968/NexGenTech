using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Shared.Models;
using System;

namespace MedHelpAuthorizations.Infrastructure.Services.PublishInfo
{
    public class PublishInformationManager : IPublishInformationManager
    {
        public ReleaseArtifactInfo PublishInfo { get; private set; }

        public PublishInformationManager(ReleaseArtifactInfo publishInfo)
        {
            PublishInfo = publishInfo ?? throw new ArgumentNullException(nameof(publishInfo));
        }

        public ReleaseArtifactInfo GetPublishInfo()
        {
            return PublishInfo;
        }
    }
}
