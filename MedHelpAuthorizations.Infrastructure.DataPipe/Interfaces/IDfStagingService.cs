using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.Entities;
using MedHelpAuthorizations.Infrastructure.DataPipe.Models;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces
{
	public interface IDfStagingService : IService
	{
		Task<List<T>> GetUnprocessedDfRecordsByTenantClient<T>(string tenantClientString) where T : DfStagingAuditableEntity;

		Task UpdateDfProcessedSuccessfully<T>(int id, CancellationToken cancellationToken) where T : DfStagingAuditableEntity;

		Task UpdateDfProcessedSuccessfully<T>(T dfStagingEntity, CancellationToken cancellationToken) where T : DfStagingAuditableEntity;

		Task UpdateDfProcessedError<T>(int id, string errorMessage, CancellationToken cancellationToken) where T : DfStagingAuditableEntity;

		Task UpdateDfProcessedError<T>(T dfStagingEntity, string errorMessage, CancellationToken cancellationToken) where T : DfStagingAuditableEntity;

		Task TransformTblAdjustmentCodes(List<TblAdjustmentCode> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblCardholders(List<TblCardHolder> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository);

		Task TransformTblCharges(List<TblCharge> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblClaimAdjustments(List<TblClaimAdjustment> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblClaimPayments(List<TblClaimPayment> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblInsurances(List<TblInsurance> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblLocations(List<TblLocation> records, int clientId, IUnitOfWork<int> unitOfWork, IAddressRepository addressRepository);

		Task TransformTblPatientInsuranceCards(List<TblPatientInsuranceCard> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblPatients(List<TblPatient> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository);

		Task TransformTblPlaceOfServices(List<TblPlaceOfService> records, int clientId, IUnitOfWork<int> unitOfWork, IAddressRepository addressRepository);

		Task TransformTblProviderLocations(List<TblProviderLocation> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblProviders(List<TblProvider> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository);

		Task TransformTblRemittances(List<TblRemittance> records, int clientId, IUnitOfWork<int> unitOfWork);

		Task TransformTblResponsibleParties(List<TblResponsibleParty> records, int clientId, IUnitOfWork<int> unitOfWork, IPersonRepository personRepository, IAddressRepository addressRepository);
	}
}
