using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class ApplicationEndpoints
    {
        public static string GetApplicationFeatures = "api/v1/tenant/application/applicationFeatures";
        public static string GetApiKeys = "api/v1/tenant/application/apiKeys";
    }
}
