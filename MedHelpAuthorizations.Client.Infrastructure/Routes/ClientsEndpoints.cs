using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetClientsByCriteria;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
	class ClientsEndpoint
	{
		public static string GetAllPaged(int pageNumber, int pageSize)
		{
			return $"api/v1/tenant/clients?pageNumber={pageNumber}&pageSize={pageSize}";
		}
		public static string GetById(int id)
		{
			//api/v1/tenant/clients/3
			return $"api/v1/tenant/clients/{id}";
		}

		public static string GetByCriteriaPaged(GetClientsByCriteriaQuery query)
		{
			return $"api/v1/tenant/clients/Search?pageNumber={query.PageNumber}&pageSize={query.PageSize}&ClientCode={query.ClientCode}&" +
				$"Name={query.Name}";
		}

		public static string GetCount = "api/v1/tenant/clients/count";

		public static string GetClientImage(int clientId)
		{
			return $"api/v1/tenant/clients/image/{clientId}";
		}

		public static string Save = "api/v1/tenant/clients";
		// public static string SaveClientKpi = "api/v1/tenant/clients/addClientkpi";
		public static string Delete = "api/v1/tenant/clients";
		public static string Export = "api/v1/tenant/clients/export";
		public static string DeleteClientConfig = "api/v1/tenant/clients/clientConfig";
		public static string GetAllClients()
		{
			return "api/v1/tenant/clients";
		}

        public static string GetCurrentClientData()
        {
            return "api/v1/tenant/clients/CurrentClientData";
        }

        //public static string GetByClientId(int clientId)
        //{
        //	return $"api/v1/tenant/clients/getByClientId/{clientId}";
        //}
    }
}
