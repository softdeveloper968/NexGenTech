using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{

    public static class UserAlertEndpoints
    {      

        public static string GetById(int alertId)
        {
            return $"api/v1/tenant/UserAlert/{alertId}";
        }

        public static string GetByUserId(string userId, int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/UserAlert/ByUser/{userId}?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string Save = "api/v1/tenant/UserAlert";
        public static string Delete = "api/v1/tenant/UserAlert";
    }
}

