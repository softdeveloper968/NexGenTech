using System;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class ClientFeeScheduleEndPoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/ClientFeeSchedule/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string Save = "api/v1/tenant/ClientFeeSchedule";
        public static string Delete = "api/v1/tenant/ClientFeeSchedule";

        public static string GetByInsuranceId(int clientInsuranceId, int clientId)
        {
            return $"api/v1/tenant/ClientFeeSchedule/{clientInsuranceId}?clientId={clientId}";
        }

        public static string GetAllFeeSchedule()
        {
            return "api/v1/tenant/ClientFeeSchedule/getAllCLientFeeSchedule";
        }

        public static string GetAllByCriteria(int clientInsuranceId, DateTime dateOfService)
        {
            return $"api/v1/tenant/ClientFeeSchedule/criteria?clientInsuranceId={clientInsuranceId}&dateOfService={dateOfService}";
        }


	}
}
