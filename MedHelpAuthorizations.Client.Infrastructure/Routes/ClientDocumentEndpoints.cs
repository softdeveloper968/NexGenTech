using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientDocumentEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"api/v1/tenant/ClientDocument/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }
        public static string Save = "api/v1/tenant/ClientDocument";
        public static string Delete = "api/v1/tenant/ClientDocument";
    }
}
