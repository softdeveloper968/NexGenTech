using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class FeeScheduleEntryEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, int clientFeeScheduleId, string searchString, string sortLabel, string sortDirection)
        {
            return $"api/v1/tenant/FeeScheduleEntry/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&clientFeeScheduleId={clientFeeScheduleId}&searchString={searchString}&sortLabel={sortLabel}&sortDirection={sortDirection}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/ClientInsurance/{id}";
        }

        public static string Save = "api/v1/tenant/FeeScheduleEntry";
		public static string SaveImportData = "api/v1/tenant/FeeScheduleEntry/ImportEntry";
		public static string SaveCopyData = "api/v1/tenant/FeeScheduleEntry/copyClientFeeScheduleEntry";
		public static string Delete = "api/v1/tenant/FeeScheduleEntry";
        public static string GetAllFeeScheduleEntry()
        {
            return "api/v1/tenant/FeeScheduleEntry/Entry";
        }

		public static string GetByClientFeeScheduleId(int clientFeeScheduleId)
		{
			return $"api/v1/tenant/FeeScheduleEntry/clientFeeSchedule?clientFeeScheduleId={clientFeeScheduleId}";
		}

		public static string AutoCreateFeeSchedule = "api/v1/tenant/FeeScheduleEntry/autoCreateFeeSchedule";
	}
}
