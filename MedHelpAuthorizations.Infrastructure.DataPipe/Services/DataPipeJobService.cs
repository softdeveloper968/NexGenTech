using AutoMapper;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.DfStaging;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.Entities;
using MedHelpAuthorizations.Infrastructure.DataPipe.Extensions;
using MedHelpAuthorizations.Infrastructure.DataPipe.Interfaces;
using MedHelpAuthorizations.Infrastructure.DataPipe.Models;
using MedHelpAuthorizations.Shared.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RazorHtmlEmails.RazorClassLib.Services;
using System.Reflection;
using IClientRepository = MedHelpAuthorizations.Application.Interfaces.Repositories.IClientRepository;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Services
{
	public partial class DataPipeJobService : IDataPipeJobService
	{
		private readonly IDfStagingService _dfStagingService;
		private readonly IDfStagingUnitOfWork _dfStagingUnitOfWork;
		private readonly ITenantCryptographyService _tenantCryptographyService;
		private readonly IMailService _mailService;
		private readonly ILogger<IDataPipeJobService> _logger;
		private readonly IConfiguration _configuration;
		private readonly IExcelService _excelService;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		private readonly ITenantManagementService _tenantManagementService;
		private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
		private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
		public DataPipeJobService(
			ITokenService identityService,
			IDfStagingService dfStagingService,
			IDfStagingUnitOfWork dfStagingUnitOfWork,
			ITenantCryptographyService tenantCryptographyService,
			IMailService mailService,
			ILogger<IDataPipeJobService> logger,
			IConfiguration configuration,
			IExcelService excelService,
			IMediator mediator,
			IMapper mapper,
			ITenantManagementService tenantManagementService,
			ITenantRepositoryFactory tenantRepositoryFactory,
			IRazorViewToStringRenderer razorViewToStringRenderer)
		{
			_dfStagingService = dfStagingService;
			_dfStagingUnitOfWork = dfStagingUnitOfWork;
			_tenantCryptographyService = tenantCryptographyService;
			_mailService = mailService;
			_logger = logger;
			_configuration = configuration;
			_excelService = excelService;
			_mediator = mediator;
			_mapper = mapper;
			_tenantManagementService = tenantManagementService;
			_tenantRepositoryFactory = tenantRepositoryFactory;
			_razorViewToStringRenderer = razorViewToStringRenderer;
		}

		/// <summary>
		/// Transforms Data in DfStaging Database into usable data in AIT FA database
		/// </summary>
		/// <returns>A boolean indicating the success of the operation.</returns>
		public async Task<bool> DoTransformDfStagingRecords()
		{
			try
			{
				List<IDfStagingAuditableEntity> myClasses = new List<IDfStagingAuditableEntity>();

				//Get all the types that inherit from DfStagingAuditableEntity
				var dfStagingEntityTypes = ReflectiveEnumerator.GetDerivedTypeList<DfStagingAuditableEntity>();
                // Order the list by table dependency order (read from a hardcoded attribute "ImportOrderAttribute" that is hardcoded on each type 
                // Order Should be:
				// 1 AdjustmentCodes   2 Locations   3Insurance   4 Place Of Service   5 Provider   6 ProviderLocation   7Responsible Party   8 Patient   9 Cardholder   10 Patient Insurance Card   11 Remittance   12  Charges   13 Payments   14 Adjustments
                dfStagingEntityTypes = dfStagingEntityTypes.OrderByImportOrder();

				// Retrieve all tenants
				var tenants = await _tenantManagementService.GetAllActiveAsync();
				foreach (var tenant in tenants ?? new List<TenantDto>())
				{
					// Get repositories for Tenant
					var _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenant.Identifier);
					var _personRepository = _tenantRepositoryFactory.GetPersonRepository(tenant.Identifier);
                    var _addressRepository = _tenantRepositoryFactory.GetAddressRepository(tenant.Identifier);

                    // Get Clients in tenant that have datapipe integration 
                    var _clientRepository = await _tenantRepositoryFactory.GetAsync<IClientRepository>(tenant.Identifier);
					var clients = await _clientRepository.GetAllDataPipeFeatureClients();


					foreach (var client in clients)
					{
						// Get TenantClientString for the client 
						var tenantClientString = _tenantCryptographyService.Encrypt(tenant.Identifier, client.Id);
						//var tenantClientString = "96C4ABB3AB5489C8A620453C1C230FF5BB6A843ACE106DFD3869D63DF0831934";
						if (string.IsNullOrEmpty(tenantClientString))
						{
							//TODO: SEND EMail that TenantClientString could not be created
							continue; 
						}

						//Loop through dependency ordered dfStagingEntityTypes and get unprocessed records 
						foreach (var type in dfStagingEntityTypes)
						{
							var unprocessedRecords = await GetUnprocessedRecords(type, _dfStagingService, tenantClientString);
							if (unprocessedRecords == null)
								continue;

							switch (unprocessedRecords)
							{
								case List<TblAdjustmentCode> adjustmentCodes:
									await _dfStagingService.TransformTblAdjustmentCodes(adjustmentCodes, client.Id, _unitOfWork);
									break;

                                case List<TblLocation> locations:
                                    await _dfStagingService.TransformTblLocations(locations, client.Id, _unitOfWork, _addressRepository);
                                    break;

                                case List<TblInsurance> insurances:
                                    await _dfStagingService.TransformTblInsurances(insurances, client.Id, _unitOfWork);
                                    break;
                                case List<TblPlaceOfService> placesOfService:

                                    await _dfStagingService.TransformTblPlaceOfServices(placesOfService, client.Id, _unitOfWork, _addressRepository);
                                    break;

                                case List<TblProvider> providers:
                                    await _dfStagingService.TransformTblProviders(providers, client.Id, _unitOfWork, _personRepository, _addressRepository);
                                    break;

                                case List<TblProviderLocation> providerLocations:
                                    await _dfStagingService.TransformTblProviderLocations(providerLocations, client.Id, _unitOfWork);
                                    break;

                                case List<TblResponsibleParty> responsibleParties:
                                    await _dfStagingService.TransformTblResponsibleParties(responsibleParties, client.Id, _unitOfWork, _personRepository, _addressRepository);
                                    break;

                                case List<TblPatient> patients:
                                    await _dfStagingService.TransformTblPatients(patients, client.Id, _unitOfWork, _personRepository, _addressRepository);
                                    break;

                                case List<TblCardHolder> cardholders:
									await _dfStagingService.TransformTblCardholders(cardholders, client.Id, _unitOfWork, _personRepository, _addressRepository);
									break;

                                case List<TblPatientInsuranceCard> insuranceCards:
                                    await _dfStagingService.TransformTblPatientInsuranceCards(insuranceCards, client.Id, _unitOfWork);
                                    break;

                                case List<TblRemittance> remittances:
                                    await _dfStagingService.TransformTblRemittances(remittances, client.Id, _unitOfWork);
                                    break;

                                case List<TblCharge> charges:
									await _dfStagingService.TransformTblCharges(charges, client.Id, _unitOfWork);
									break;

                                case List<TblClaimPayment> payments:
                                    await _dfStagingService.TransformTblClaimPayments(payments, client.Id, _unitOfWork);
                                    break;

                                case List<TblClaimAdjustment> adjustments:
									await _dfStagingService.TransformTblClaimAdjustments(adjustments, client.Id, _unitOfWork);
									break;

								default:
									throw new Exception($"Returned records of type: {type.GetType().FullName} are missing a transform method call in switch/case");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Log errors related to retrieving or processing unresolved claim status batches
				_logger.LogError("Failed getting unresolved claim status batches. Error - " + ex.Message);
				return false;
			}

			return true; // Operation completed successfully // 
		}

		private async Task<object?> GetUnprocessedRecords(Type dfStgType, IDfStagingService dfStgService, string tenantClientString)
		{
			//Get type of the instance
			//Type type = dfStgType.GetType();

			// Get the method info
			MethodInfo method = typeof(DfStagingService).GetMethod(nameof(DfStagingService.GetUnprocessedDfRecordsByTenantClient));

			// Make the generic method
			MethodInfo genericMethod = method.MakeGenericMethod(dfStgType);

			// Call the method and await the result
			var task = genericMethod.Invoke(dfStgService, new object[1] { tenantClientString }) as Task;
			await task;

			// Get the result from the task
			var resultProperty = task.GetType().GetProperty("Result");
			object? result = resultProperty.GetValue(task);

			return result;
		}
	}
}