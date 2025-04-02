namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
	public static class ImportDocumentMessageEndPoints
	{
		public static string GetAllPaged(int pageNumber, int pageSize, int inputDocumentId, int messageType)
		{
			return $"api/v1/tenant/ImportDocumentMessage/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&inputDocumentId={inputDocumentId}&messageType={messageType}";
		}
		public static string GetByInputDcocumentId(int inputDocumentId)
		{
			return $"api/v1/tenant/ImportDocumentMessage/{inputDocumentId}";
		}

		public static string GetExportdata(int inputDocumentId, string messageType)
		{
			return $"api/v1/tenant/ImportDocumentMessage/ExportInputDocumentMessage?inputDocumentId={inputDocumentId}&messageType={messageType}";
		}
	}
}
