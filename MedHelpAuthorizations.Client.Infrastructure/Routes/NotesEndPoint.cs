using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class NotesEndpoints
    {
        public static string GetById(int id)
        {
            return $"{Get}/{id}";
        }

        public static string BelongsTo(int id)
        {
            return $"{Get}/BelongsTo/{id}";
        }
        public static string GetByAuth(int aid)
        {
            return $"{Get}/ByAuth/{aid}";
        }
        public static string Get = "api/v1/tenant/Note";
        public static string Save = "api/v1/tenant/Note";
        public static string Delete = "api/v1/tenant/Note";
    }
}
