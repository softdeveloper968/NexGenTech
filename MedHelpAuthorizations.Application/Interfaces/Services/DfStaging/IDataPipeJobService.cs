namespace MedHelpAuthorizations.Application.Interfaces.Services.DfStaging
{
	public interface IDataPipeJobService
	{
		Task<bool> DoTransformDfStagingRecords();
	}
}