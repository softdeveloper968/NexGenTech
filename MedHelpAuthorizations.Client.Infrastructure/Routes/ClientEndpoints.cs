using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientEndpoints
    {
        public static string GetById(int id)
        {
            return $"{Get}/{id}";
        }
        public static string Get = "api/v1/Clients";
        public static string Save = "api/v1/Clients";
        public static string Delete = "api/v1/Clients";
    }
}
