namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class ClientInsuranceFeeScheduleEndPoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, int clientInsuranceId)
        {
            return $"api/v1/tenant/ClientInsuranceFeeSchedule/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&clientInsuranceId={clientInsuranceId}";
        }

        public static string GetAllByClientFeeScheduleId(int clientFeeScheduleId)
        {
            return $"api/v1/tenant/ClientInsuranceFeeSchedule/clientFeeScheduleId?clientFeeScheduleId={clientFeeScheduleId}";
        }

        public static string Save = "api/v1/tenant/ClientInsuranceFeeSchedule";
        public static string Update = "api/v1/tenant/ClientInsuranceFeeSchedule/update";
    }
}
