using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class DocumentTypeEndPoints
    {
        public static string GetAllPaged = "api/v1/tenant/DocumentType";

        public static string Save = "api/v1/tenant/DocumentType";

        public static string Export = "api/v1/tenant/DocumentType/export";

        public static string Delete(int id) => $"api/v1/tenant/DocumentType/{id}";

    }
}
