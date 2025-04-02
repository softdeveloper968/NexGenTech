using MedHelpAuthorizations.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Common
{
    public interface IPublishInformationManager
    {
        ReleaseArtifactInfo GetPublishInfo();
    }
}
