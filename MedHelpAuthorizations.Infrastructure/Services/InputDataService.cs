using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    using Hangfire.Dashboard;
    using MedHelpAuthorizations.Application.Interfaces.Common;
    using MedHelpAuthorizations.Application.Interfaces.Repositories;

    using MedHelpAuthorizations.Domain.Entities;
    using MedHelpAuthorizations.Domain.Entities.Enums;
    using MedHelpAuthorizations.Shared.Helpers;
    using Microsoft.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Identity.Client;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Threading;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class InputDataService : IInputDataService
    {
        private readonly IMapper _mapper;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IClientLocationRepository _clientLocationRepository;
        private readonly IClientCptCodeRepository _clientCptCodeRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        private readonly IPatientRepository _patientRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IClientFeeScheduleService _processFeeScheduleMatchedClaimService; //AA-315
        private readonly IUnMappedFeeScheduleCptRepository _unMappedFeeScheduleCptRepository;  //EN-155
        private IList<ClaimStatusBatchClaim> _batchClaims { get; set; }
        private readonly IClaimNumberNormalizationService _claimNumberNormalization; //EN-35
        private readonly ICptCodeRepository _cptCodeRepository; //EN-447
        private readonly IHubService _hubService;
        public InputDataService(IMapper mapper, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService,
            IProviderRepository providerRepository, IClientLocationRepository clientLocationRepository, IPatientRepository patientRepository, IPersonRepository personRepository, IClaimNumberNormalizationService claimNumberNormalization, IClientCptCodeRepository clientCptCodeRepository, IClientFeeScheduleService writeOffService,
            IUnMappedFeeScheduleCptRepository unMappedFeeScheduleCptRepository, ICptCodeRepository cptCodeRepository, IHubService hubService)
        {
            _mapper = mapper;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _providerRepository = providerRepository;
            _clientLocationRepository = clientLocationRepository;
            _batchClaims = new List<ClaimStatusBatchClaim>();
            _patientRepository = patientRepository;
            _personRepository = personRepository;
            _claimNumberNormalization = claimNumberNormalization;

            _clientCptCodeRepository = clientCptCodeRepository;
            _unMappedFeeScheduleCptRepository = unMappedFeeScheduleCptRepository;
            _processFeeScheduleMatchedClaimService = writeOffService;
            _cptCodeRepository = cptCodeRepository;
            _hubService = hubService;
        }

        public Task<IList<ClaimStatusBatchClaim>> DeserializeClaimStatusInputDataAsync(string inputDocumentUrl)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Deserializes an input byte array of ("csv"... more to come) file type(s). 
        /// </summary>
        /// <param name="inputDocumentBytes">
        /// </param>
        /// <param name="fileExtension">
        /// </param>
        /// <exception cref="FormatException">
        /// File extension must be either 'csv', 'xls', or 'xlsx'
        /// </exception>
        /// <returns>
        /// Collection of ClaimStatusBatchClaims from csv byte array input
        /// </returns>
        public async Task<Tuple<IList<ClaimStatusBatchClaim>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, int>> 
            DeserializeClaimStatusInputDataAsync(IList<ClaimStatusBatchClaim> batchClaims,
                                                 IList<ClaimStatusBatchClaimModel> erroredBatchClaims,
                                                 IList<ClaimStatusBatchClaimModel> unmatchedLocationBatchClaims,
                                                 IList<ClaimStatusBatchClaimModel> unmatchedProviderBatchClaims,
                                                 IList<ClaimStatusBatchClaimModel> filesDuplicates,
                                                 IList<ClaimStatusBatchClaimModel> unsupplantableDuplicates,
												 int attemptedImportCount,
												 int clientInsuranceId,
                                                 byte[] inputDocumentBytes,
                                                 string fileExtension, int inputDocumentId
                                                 )
        {

            var normalizedFileExtension = fileExtension?.Replace(".", string.Empty).ToLower().Trim() ?? string.Empty;
			IList<ClaimStatusBatchClaim> deserializedBatchClaims = new List<ClaimStatusBatchClaim>();
			//ICollection<ClaimStatusBatchClaim> batchClaims = null;
			switch (normalizedFileExtension)
            {
                case "csv":
                    var result = await DeserializeCsvClaimStatusInputDataAsync(inputDocumentBytes, clientInsuranceId, erroredBatchClaims, unmatchedLocationBatchClaims, unmatchedProviderBatchClaims, inputDocumentId);
					deserializedBatchClaims = result.Item1;
					attemptedImportCount = result.Item2;
					break;
                case "xls":
                    break;
                case "xlsx":
                    break;
                default:
                    throw new System.FormatException("File extension must be either 'csv', 'xls', or 'xlsx'");
            }

			List<ClaimStatusBatchClaim> batchClaimsCollection = deserializedBatchClaims.ToList();
			
			List<ClaimStatusBatchClaim> claimsToBeRemoved = ProcessProcedureCodesAsync(ref batchClaimsCollection, clientInsuranceId, filesDuplicates, unsupplantableDuplicates, inputDocumentId);
            var scrubbedBatchClaimsList = batchClaimsCollection.Where(p => !claimsToBeRemoved.Any(p2 => p2.Equals(p))).ToList();

			return Tuple.Create<IList<ClaimStatusBatchClaim>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, int>
                    (scrubbedBatchClaimsList, erroredBatchClaims, unmatchedLocationBatchClaims, unmatchedProviderBatchClaims, filesDuplicates, unsupplantableDuplicates,  attemptedImportCount);
        }

		private async Task<Tuple<IList<ClaimStatusBatchClaim>, int>> 
                        DeserializeCsvClaimStatusInputDataAsync(byte[] inputDocumentBytes,
	                                                        int clientInsuranceId,
	                                                        IList<ClaimStatusBatchClaimModel> erroredBatchClaims,
	                                                        IList<ClaimStatusBatchClaimModel> unmatchedLocationBatchClaims,
	                                                        IList<ClaimStatusBatchClaimModel> unmatchedProviderBatchClaims, int inputDocumentId)
		{
			HashSet<ClaimStatusBatchClaimModel> batchClaimModels = new HashSet<ClaimStatusBatchClaimModel>();
			IList<ClaimStatusBatchClaim> batchClaims = new List<ClaimStatusBatchClaim>();
			int attemptedImportCount = 0;

			using (MemoryStream ms = new MemoryStream(inputDocumentBytes))
			using (StreamReader reader = new StreamReader(ms, true))
			{
				var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
				{
					HasHeaderRecord = true,
					MissingFieldFound = null
				};

				using (var csv = new CsvReader(reader, csvConfig))
				{
					try
					{
						batchClaimModels = csv.GetRecords<ClaimStatusBatchClaimModel>().ToHashSet();
						attemptedImportCount = batchClaimModels.Count();

                        await _hubService.SendAttemptedCountToClient("ReceiveAttemptedCount", inputDocumentId, attemptedImportCount);

                        if (!batchClaimModels.Any())
							throw new Exception("No claims were deserialized!");

                        // Total records count for progress calculation
                        int totalRecords = batchClaimModels.Count;
                        int currentIndex = 0;
                        int previousProgress = 4;

                        foreach (var c in batchClaimModels)
						{
                            currentIndex++;

                            // Calculate new progress from 4% to 25%
                            int newProgress = 4 + (int)Math.Floor(((double)currentIndex / totalRecords) * 25);

                            // To avoid sending duplicate progress values
                            if (newProgress > previousProgress)
                            {
                                previousProgress = newProgress;
                                await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocumentId, newProgress);
                            }
                            try
                            {
								ClaimStatusBatchClaim item = new ClaimStatusBatchClaim
								{
									ClaimNumber = c.ClaimNumber,
									BilledAmount = c.BilledAmount,
									DateOfBirth = c.DateOfBirth,
									PatientFirstName = c.PatientFirstName,
									PatientLastName = c.PatientLastName,
									ProcedureCode = c.ProcedureCode?.Trim().Substring(0, 5),
									Modifiers = c.Modifiers,
									DateOfServiceFrom = c.DateOfServiceFrom,
									DateOfServiceTo = c.DateOfServiceTo,
									PolicyNumber = Regex.Replace(c.PolicyNumber, @"\s+", ""),
									ClaimBilledOn = c.ClaimBilledOn,
									Quantity = c.Quantity == null || c.Quantity == 0 ? 1 : (int)c.Quantity,
									RenderingNpi = c.RenderingNpi, // Later Remove this after CLientId is obtained and populated when we Get ProviderIdByNpi(string npi)
									GroupNpi = c.GroupNpi,
									ClientProviderId = await GetClientProviderByNpi(c.RenderingNpi), //Get ProviderIdByNpi(string npi)
									ClientLocationId = await GetClientLocationByName(c.LocationName),
									PatientId = await GetPatientId(c, new CancellationToken()),
									ClientInsuranceId = clientInsuranceId,
									ClientId = _clientId,
									NormalizedClaimNumber = await _claimNumberNormalization.GetNormalizedClaimNumber(c.ClaimNumber) //EN-35
								};

								if (item.ClientProviderId == null || item.ClientProviderId == 0)
									unmatchedProviderBatchClaims.Add(c);
								else if (item.ClientLocationId == null || item.ClientLocationId == 0)
									unmatchedLocationBatchClaims.Add(c);
								else
                                {
                                    batchClaims.Add(item);
                                    await _hubService.SendActualCountToClient("ReceiveActualCount", inputDocumentId, batchClaims.Count());
                                }
									
							}
							catch (Exception)
							{
								erroredBatchClaims.Add(c);
							}
						}
					}
					catch (Exception ex)
					{
						// Consider logging the exception or handling it accordingly
						throw;
					}
				}
			}
            // Ensure progress is set to 25% after processing all records.
            await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocumentId, 25);
            return Tuple.Create(batchClaims, attemptedImportCount);
		}


		public  List<ClaimStatusBatchClaim> ProcessProcedureCodesAsync(ref List<ClaimStatusBatchClaim> batchClaimsCollection, int clientInsuranceId, IList<ClaimStatusBatchClaimModel> filesDuplicates, IList<ClaimStatusBatchClaimModel> unsupplantableDuplicates, int inputDocumentId)
        {

            if (batchClaimsCollection.Any(x => x.ProcedureCode.Contains(',')))
            {
                var rows = batchClaimsCollection.Where(x => x.ProcedureCode.Contains(',')).ToList();
                int totalRows = rows.Count;
                int rowIndex = 0;
                int previousProgress = 26; // UI is at 25% when starting this section
                foreach (var row in rows)
                {
                    rowIndex++;
                    int progress = 26 + (int)Math.Floor(((double)rowIndex / totalRows) * (40 - 26));
                     _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocumentId, progress);

                    List<string> procedureCodes = row.ProcedureCode.Split(',').ToList();
                    foreach (var code in procedureCodes)
                    {
                        var newRow = new ClaimStatusBatchClaim
                        {
                            PatientLastName = row.PatientLastName,
                            PatientFirstName = row.PatientFirstName,
                            DateOfBirth = row.DateOfBirth,
                            PolicyNumber = row.PolicyNumber,
                            DateOfServiceFrom = row.DateOfServiceFrom,
                            DateOfServiceTo = row.DateOfServiceTo,
                            ClaimBilledOn = row.ClaimBilledOn,
                            ProcedureCode = code?.Trim().Substring(0, 5),
                            BilledAmount = row.BilledAmount,
                            IsDeleted = false,
                            RenderingNpi = row.RenderingNpi,
                            GroupNpi = row.GroupNpi,
                            ClaimNumber = row.ClaimNumber,
                            ClientInsuranceId = clientInsuranceId,
                            ClientId = _clientId,
                        };
                        batchClaimsCollection.Add(newRow);
                    }
                }

                batchClaimsCollection.RemoveAll(x => x.ProcedureCode.Contains(','));
            }

            // Group by ClaimNumber, ProcedureCode, Modifiers, and DateOfServiceFrom
            var groupedClaims = batchClaimsCollection
                .GroupBy(bc => new { bc.ClaimNumber, bc.ProcedureCode, bc.Modifiers, bc.DateOfServiceFrom })
                .ToList();


            // Identify duplicates and add them to the duplicateBatchClaims list
            foreach (var group in groupedClaims)
            {
                if (group.Count() > 1) // If there are duplicates
                {
                    // Skip the first item in the group and add the rest to the duplicate list
                    var duplicates = group.Skip(1);

                    // Add to the duplicateBatchClaims list as ClaimStatusBatchClaimModel
                    foreach (var duplicateClaim in duplicates)
                    {
                        filesDuplicates.Add(new ClaimStatusBatchClaimModel
                        {
                            ClaimNumber = duplicateClaim.ClaimNumber,
                            ProcedureCode = duplicateClaim.ProcedureCode,
                            PatientFirstName = duplicateClaim.PatientFirstName,
                            PatientLastName = duplicateClaim.PatientLastName,
                            DateOfBirth = duplicateClaim.DateOfBirth,
                            PolicyNumber = duplicateClaim.PolicyNumber,
                            DateOfServiceFrom = duplicateClaim.DateOfServiceFrom,
                            DateOfServiceTo = duplicateClaim.DateOfServiceTo,
                            Modifiers = duplicateClaim.Modifiers,
                        });
                    }
                }
            }

            // Remove any duplicates in the imported File.
            batchClaimsCollection = batchClaimsCollection
                .DistinctBy(bc => new { bc.ClaimNumber, bc.ProcedureCode, bc.Modifiers, bc.DateOfServiceFrom })
                .Select(bc => bc)
                .ToList();
            _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocumentId, 45);

            // Process claim hash and supplant
            List<ClaimStatusBatchClaim> claimsToBeRemoved = ProcessClaimHashAndSupplantAsync(batchClaimsCollection, unsupplantableDuplicates, inputDocumentId).Result;

            return claimsToBeRemoved;
        }

        private async Task<List<ClaimStatusBatchClaim>> ProcessClaimHashAndSupplantAsync(List<ClaimStatusBatchClaim> batchClaimsCollection, IList<ClaimStatusBatchClaimModel> unsupplantableDuplicates, int inputDocumentId)
        {
            List<ClaimStatusBatchClaim> claimsToBeRemoved = new List<ClaimStatusBatchClaim>();
            //TODO: Put in metrics to know how long it takes to run through this method

            // Update progress from 46% to 65% as we loop through the claims.
            int totalItems = batchClaimsCollection.Count;
            int currentItem = 0;
            foreach (var bc in batchClaimsCollection)
            {
                currentItem++;
                int progress = 46 + (int)Math.Floor(((double)currentItem / totalItems) * (65 - 46));
                await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocumentId, progress);

                var claimMd5Hash = HashHelpers.CreateClaimSql0xMD5(bc.PatientId, bc.ProcedureCode, bc.Modifiers, bc.DateOfServiceFrom.Value);
                var activeBatchClaim = await _claimStatusBatchClaimsRepository.GetActiveByEntryMd5HashAsync(claimMd5Hash);
                if(activeBatchClaim == null)
                {
                    //Lookup by claimnumber and clientId. 
                    activeBatchClaim = await _claimStatusBatchClaimsRepository.GetActiveByClaimNumberAndPatientIdAsync(bc.ClaimNumber, bc.PatientId);
                }
                string[] md5InputStrings = { bc.PatientId?.ToString()?.Trim(), "|", bc.ProcedureCode.ToUpper().Trim(), "|", bc.Modifiers?.ToUpper()?.Trim(), "|", bc.DateOfServiceFrom?.ToString("yyyyMMdd"), "|" };
                bc.CalculatedLookupHash = claimMd5Hash;
                bc.CalculatedLookupHashInput = string.Concat(md5InputStrings);

                if (activeBatchClaim != null)
                {
                    bc.OriginalClaimBilledOn = activeBatchClaim.OriginalClaimBilledOn;

                    if (await _claimStatusBatchClaimsRepository.IsSupplantableAsync(activeBatchClaim, bc))
                    {
                        await _claimStatusBatchClaimsRepository.UpdateSupplantedByIdAsync(activeBatchClaim.Id);
                    }
                    else
                    {
                        unsupplantableDuplicates.Add(new ClaimStatusBatchClaimModel
                        {
                            ClaimNumber = bc.ClaimNumber,
                            ProcedureCode = bc.ProcedureCode,
                            PatientFirstName = bc.PatientFirstName,
                            PatientLastName = bc.PatientLastName,
                            DateOfBirth = bc.DateOfBirth,
                            PolicyNumber = bc.PolicyNumber,
                            DateOfServiceFrom = bc.DateOfServiceFrom,
                            DateOfServiceTo = bc.DateOfServiceTo,
                            ClaimStatusBatchClaimId = activeBatchClaim.Id,
                        });
                        claimsToBeRemoved.Add(bc);
                    }
                }
                else
                {
					bc.OriginalClaimBilledOn = bc.ClaimBilledOn;
                }
            }
            // Ensure the progress reaches exactly 65% at the end.
            await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocumentId, 65);
            return claimsToBeRemoved;
        }


        public bool IsDuplicateSubmission()
        {
            return false;
        }

        public async Task<int?> GetClientLocationByName(string locationName)
        {
            var location = await _clientLocationRepository.FindByNameAsync(locationName);
            return location?.Id;

        }

        public async Task<int?> GetClientProviderByNpi(string renderingNpi)
        {
            var provider = await _providerRepository.FindByNpiAsync(renderingNpi);
            return provider?.Id;
        }

        /// <summary>
        /// populate patient and person tables
        /// </summary>
        /// <param name="patientInfo"></param>
        /// <returns>patient id</returns>
        public async Task<int?> GetPatientId(ClaimStatusBatchClaimModel patientInfo, CancellationToken cancellationToken)
        {
            //search for person with the given info
            var person = await _patientRepository.GetPersonByInputInfo(_clientId, patientInfo.PatientFirstName, patientInfo.PatientLastName, patientInfo.DateOfBirth);
            if (person == null)
            {
                //if no person exists with the given info
                //insert person with given info
                Person newPerson = new Person()
                {
                    FirstName = patientInfo.PatientFirstName,
                    LastName = patientInfo.PatientLastName,
                    DateOfBirth = patientInfo?.DateOfBirth,
                    ClientId = _clientId
                };
                await _unitOfWork.Repository<Person>().AddAsync(newPerson);
                await _unitOfWork.Commit(cancellationToken);

                //create patient with the new person Id as personId
                Patient newPatient = new Patient()
                {
                    AdministrativeGenderId = Domain.Entities.Enums.AdministrativeGenderEnum.Unknown,
                    ResponsiblePartyRelationshipToPatient = Domain.Entities.Enums.RelationShipTypeEnum.Other,
                    ClientId = _clientId,
                    PersonId = newPerson.Id
                };
                await _unitOfWork.Repository<Patient>().AddAsync(newPatient);
                await _unitOfWork.Commit(cancellationToken);
                return newPatient.Id;
            }
            else
            {
                var patient = await _patientRepository.GetpatientByInputInfo(person.Id);
                if (patient == null)
                {
                    //if no patient exists with the personId
                    //create new patient with the personId
                    Patient newPatient = new Patient()
                    {
                        AdministrativeGenderId = Domain.Entities.Enums.AdministrativeGenderEnum.Unknown,
                        ResponsiblePartyRelationshipToPatient = Domain.Entities.Enums.RelationShipTypeEnum.Other,
                        ClientId = _clientId,
                        PersonId = person.Id
                    };
                    await _unitOfWork.Repository<Patient>().AddAsync(newPatient);
                    await _unitOfWork.Commit(cancellationToken);
                    return newPatient.Id;
                }
                else
                {
                    //person found with the given info
                    //patient found having personId as person's Id
                    return patient.Id;
                }
            }
        }

        /// <summary>
        /// get batch claims for writeOffs
        /// </summary>
        /// <param name="clientInsuranceId"></param>
        /// <param name="inputDocumentBytes"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<IList<ClaimStatusWriteOffs>> DeserializeClaimStatusWriteOffDataAsync(byte[] inputDocumentBytes, string fileExtension)
        {
            var normalizedFileExtension = fileExtension?.Replace(".", string.Empty).ToLower().Trim() ?? string.Empty;

            ICollection<ClaimStatusWriteOffs> batchClaimsCollection = null;

            switch (normalizedFileExtension)
            {
                case "csv":
                    batchClaimsCollection = await this.DeserializeCsvClaimStatusWriteOffDataAsync(inputDocumentBytes).ConfigureAwait(true);
                    break;
                case "xls":
                    break;
                case "xlsx":
                    break;
                default:
                    throw new System.FormatException("File extension must be either 'csv', 'xls', or 'xlsx'");
            }

            try
            {
                //Remove any duplicates in the imported File. 
                batchClaimsCollection = batchClaimsCollection.DistinctBy(bc => new
                {
                    bc.ClaimNumber,
                    bc.ProcedureCode,
                    bc.Modifiers,
                    bc.DateOfServiceFrom
                }).Select(bc => bc).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return batchClaimsCollection.ToList() ?? new List<ClaimStatusWriteOffs>();
        }

        /// <summary>
        /// deserialize input data for write offs
        /// </summary>
        /// <param name="inputDocumentBytes"></param>
        /// <param name="clientInsuranceId"></param>
        /// <returns></returns>
        private async Task<IList<ClaimStatusWriteOffs>> DeserializeCsvClaimStatusWriteOffDataAsync(byte[] inputDocumentBytes)
        {

            HashSet<ClaimStatusWriteOffs> writeOffClaimModels = null;

            using (MemoryStream ms = new MemoryStream(inputDocumentBytes))
            {
                using (StreamReader reader = new StreamReader(ms, true))
                {
                    var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = true, MissingFieldFound = null };

                    using (var csv = new CsvReader(reader, csvConfig))
                    {
                        try
                        {
                            writeOffClaimModels = csv.GetRecords<ClaimStatusWriteOffs>().ToHashSet();
                            if (!writeOffClaimModels.Any())
                                throw new Exception("No claims were deserialized!");
                            foreach (ClaimStatusWriteOffs c in writeOffClaimModels)
                            {

                                c.ProcedureCode = c.ProcedureCode?.Trim().Substring(0, 5);
                            };
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            }
            return await Task.FromResult(writeOffClaimModels.ToList()).ConfigureAwait(true);
        }


        /// <summary>
        /// Deserializes the client fee schedule entry data based on the given file extension.
        /// </summary>
        /// <param name="inputDocumentBytes">The byte array representing the input document data.</param>
        /// <param name="fileExtension">The file extension of the input document.</param>
        /// <returns>A list of deserialized client fee schedule entries.</returns>
        public async Task<IList<ClientFeeScheduleEntryCsvViewModel>> DeserializeClientFeeScheduleEntryDataAsync(byte[] inputDocumentBytes, string fileExtension)
        {
            var normalizedFileExtension = fileExtension?.Replace(".", string.Empty).ToLower().Trim() ?? string.Empty;

            ICollection<ClientFeeScheduleEntryCsvViewModel> ClientFeeScheduleEntry = null;

            switch (normalizedFileExtension)
            {
                case "csv":
                    ClientFeeScheduleEntry = await this.DeserializeCsvClientFeeScheduleEntryDataAsync(inputDocumentBytes).ConfigureAwait(true);
                    break;
                case "xls":
                    break;
                case "xlsx":
                    break;
                default:
                    throw new System.FormatException("File extension must be either 'csv', 'xls', or 'xlsx'");
            }
            return ClientFeeScheduleEntry.ToList() ?? new List<ClientFeeScheduleEntryCsvViewModel>();
        }

        /// <summary>
        /// Deserializes CSV client fee schedule entry data from the provided byte array.
        /// </summary>
        /// <param name="inputDocumentBytes">The byte array representing the input CSV document.</param>
        /// <returns>A list of deserialized client fee schedule entries.</returns>
        private async Task<IList<ClientFeeScheduleEntryCsvViewModel>> DeserializeCsvClientFeeScheduleEntryDataAsync(byte[] inputDocumentBytes)
        {

            HashSet<ClientFeeScheduleEntryCsvViewModel> ClientFeeScheduleEntryCsvViewModel = null;

            using (MemoryStream ms = new MemoryStream(inputDocumentBytes))
            {
                using (StreamReader reader = new StreamReader(ms, true))
                {
                    var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = true, MissingFieldFound = null };

                    using (var csv = new CsvReader(reader, csvConfig))
                    {
                        try
                        {
                            ClientFeeScheduleEntryCsvViewModel = csv.GetRecords<ClientFeeScheduleEntryCsvViewModel>().ToHashSet();
                            if (!ClientFeeScheduleEntryCsvViewModel.Any())
                                throw new Exception("No enteries were deserialized!");
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            }
            return await Task.FromResult(ClientFeeScheduleEntryCsvViewModel.ToList()).ConfigureAwait(true);
        }

        public async Task<ClaimStatusBatch> ProcessClaimStatusBatchClaims(InputDocument inputDocument, IList<ClaimStatusBatchClaim> claimStatusBatchClaims, int? authTypeId, CancellationToken cancellationToken)
        {
            try
            {
                int previousProgress = 70;
                int claimStatusBatchClaimsCount = claimStatusBatchClaims.Count;
                int claimIndex = 0;

                foreach (var claim in claimStatusBatchClaims)
                {
                    try
                    {
                        // Update progress as we add claims to each batch
                        int progress = 70 + (int)Math.Floor(((double)claimIndex / claimStatusBatchClaimsCount) * (75 - 70)); // Update from 70% to 75%
                        if (progress > previousProgress)
                        {
                            previousProgress = progress;
                            await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocument.Id, progress);
                        }

                        var clientCptCode = await _clientCptCodeRepository.GetByClientId(claim.ClientId, claim.ProcedureCode) ?? null;
                        if (clientCptCode == null)
                        {
                            //Get all cpt code data
                            var cptCodeByProcedureCode = await _cptCodeRepository.GetByCptCode(claim.ProcedureCode) ?? null;
                            // if not null then create new one same current clientid
                            if (cptCodeByProcedureCode != null)
                            {
                                var cptCode = new ClientCptCode()
                                {
                                    ClientId = _clientId,
                                    Code = cptCodeByProcedureCode.Code,
                                    LookupName = cptCodeByProcedureCode.Description,
                                    Description = cptCodeByProcedureCode.Description,
                                    ScheduledFee = claim.BilledAmount,
                                };

                                await _unitOfWork.Repository<ClientCptCode>().AddAsync(cptCode);
                                await _unitOfWork.Commit(cancellationToken);

                                claim.ClientCptCodeId = cptCode.Id;
                            }
                            ///To do: need to discuss later on.
                        }
                        else
                        {
                            claim.ClientCptCodeId = clientCptCode.Id;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }

                // If batchID is not null.. lookup the batch 
                //.and attach the claims to it..; if not create a new batch objkiect and do WhiteSpaceCharsAttribute you do HasHeaderRecordAttribute. 
                // Create claim status batch
                var claimStatusBatch = new ClaimStatusBatch()
                {
                    ClientId = _clientId,
                    AuthTypeId = authTypeId,
                    InputDocumentId = inputDocument.Id,
                    ClientInsuranceId = inputDocument.ClientInsuranceId ?? 0,

                    ClaimStatusBatchClaims = claimStatusBatchClaims,
                    ClaimStatusBatchHistories = new List<ClaimStatusBatchHistory>()
                    {
                        new ClaimStatusBatchHistory()
                        {
                            ClientId = _clientId,
                            AuthTypeId = authTypeId,
                            ClientInsuranceId = inputDocument.ClientInsuranceId ?? 0,
                            DbOperationId = DbOperationEnum.Insert,
                            AllClaimStatusesResolvedOrExpired = false
                        }
                    }
                };

                return claimStatusBatch ?? null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<ImportDocumentMessage>> CreateImportDocumentMessages(List<ClaimStatusBatchClaimModel> erroredBatchClaims, List<ClaimStatusBatchClaimModel> unmatchedProviderBatchClaims, List<ClaimStatusBatchClaimModel> unmatchedLocationBatchClaims, int claimStatusBatchId, int inputDocumentId, List<ClaimStatusBatchClaimModel> filesDuplicates, List<ClaimStatusBatchClaimModel> unsupplantableDuplicates)
        {
            try
            {
                var importDocumentMessages = new List<ImportDocumentMessage>();

                // Iterate through erroredBatchClaims and add messages
                foreach (var bc in erroredBatchClaims)
                {
                    var errorMessage = $"Exception thrown while importing - Claim#: {bc.ClaimNumber};  Patient: {bc.PatientLastName}, {bc.PatientFirstName};  DOB: {bc.DateOfBirth.Value.ToString("MM/dd/yyyy")};  Policy: {bc.PolicyNumber};  Procedure: {bc.ProcedureCode};  DOS: {bc.DateOfServiceFrom.Value.ToString("MM/dd/yyyy")}";

                    // Create an ImportDocumentMessage and add it to the list
                    var importDocumentMessage = new ImportDocumentMessage
                    {
                        ClaimStatusBatchId = claimStatusBatchId,
                        InputDocumentId = inputDocumentId,
                        MessageType = InputDocumentMessageTypeEnum.Errored,
                        Message = errorMessage
                    };

                    importDocumentMessages.Add(importDocumentMessage);
                }

                // Iterate through unmatchedProviderBatchClaims and add messages
                foreach (var bc in unmatchedProviderBatchClaims)
                {
                    var errorMessage = $"NPI: {bc.RenderingNpi} not matched  - Claim#: {bc.ClaimNumber};  Patient: {bc.PatientLastName}, {bc.PatientFirstName};  DOB: {bc.DateOfBirth.Value.ToString("MM/dd/yyyy")};  Policy: {bc.PolicyNumber};  Procedure: {bc.ProcedureCode};  DOS: {bc.DateOfServiceFrom.Value.ToString("MM/dd/yyyy")}";

                    // Create an ImportDocumentMessage and add it to the list
                    var importDocumentMessage = new ImportDocumentMessage
                    {
                        ClaimStatusBatchId = claimStatusBatchId,
                        InputDocumentId = inputDocumentId,
                        MessageType = InputDocumentMessageTypeEnum.UnmatchedProvider,
                        Message = errorMessage
                    };

                    importDocumentMessages.Add(importDocumentMessage);
                }

                // Iterate through unmatchedLocationBatchClaims and add messages
                foreach (var bc in unmatchedLocationBatchClaims)
                {
                    var errorMessage = $"Location: {bc.LocationName} not matched  - Claim#: {bc.ClaimNumber};  Patient: {bc.PatientLastName}, {bc.PatientFirstName};  DOB: {bc.DateOfBirth.Value.ToString("MM/dd/yyyy")};  Policy: {bc.PolicyNumber};  Procedure: {bc.ProcedureCode};  DOS: {bc.DateOfServiceFrom.Value.ToString("MM/dd/yyyy")}";

                    // Create an ImportDocumentMessage and add it to the list
                    var importDocumentMessage = new ImportDocumentMessage
                    {
                        ClaimStatusBatchId = claimStatusBatchId,
                        InputDocumentId = inputDocumentId,
                        MessageType = InputDocumentMessageTypeEnum.UnmatchedLocation,
                        Message = errorMessage
                    };

                    importDocumentMessages.Add(importDocumentMessage);
                }

                // Iterate through filesDuplicates and add messages
                foreach (var bc in filesDuplicates)
                {
                    var errorMessage = $"File Duplicate - Claim#: {bc.ClaimNumber};  Patient: {bc.PatientLastName}, {bc.PatientFirstName};  DOB: {bc.DateOfBirth.Value.ToString("MM/dd/yyyy")};  Policy: {bc.PolicyNumber};  Procedure: {bc.ProcedureCode};  DOS: {bc.DateOfServiceFrom.Value.ToString("MM/dd/yyyy")}; Modifiers {bc.Modifiers}";

                    // Create an ImportDocumentMessage and add it to the list
                    var importDocumentMessage = new ImportDocumentMessage
                    {
                        ClaimStatusBatchId = claimStatusBatchId,
                        InputDocumentId = inputDocumentId,
                        MessageType = InputDocumentMessageTypeEnum.FileDuplicates,
                        Message = errorMessage
                    };

                    importDocumentMessages.Add(importDocumentMessage);
                }

                // Iterate through unsupplantableDuplicates and add messages
                foreach (var bc in unsupplantableDuplicates)
                {
                    var errorMessage = $"Unsupplantable Duplicate - Claim#: {bc.ClaimNumber};  Patient: {bc.PatientLastName}, {bc.PatientFirstName};  DOB: {bc.DateOfBirth.Value.ToString("MM/dd/yyyy")};  Policy: {bc.PolicyNumber};  Procedure: {bc.ProcedureCode};  DOS: {bc.DateOfServiceFrom.Value.ToString("MM/dd/yyyy")};";

                    // Create an ImportDocumentMessage and add it to the list
                    var importDocumentMessage = new ImportDocumentMessage
                    {
                        ClaimStatusBatchId = claimStatusBatchId,
                        InputDocumentId = inputDocumentId,
                        MessageType = InputDocumentMessageTypeEnum.UnSupplantableDuplicates,
                        Message = errorMessage,
                        ClaimStatusBatchClaimId = bc.ClaimStatusBatchClaimId,
                    };

                    importDocumentMessages.Add(importDocumentMessage);
                }

                return importDocumentMessages;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task ProcessClaimStatusBatches(InputDocument inputDocument, CancellationToken cancellationToken)
        {
            if (inputDocument?.ClaimStatusBatches?.Any() ?? false)
            {
                var claimStatusBatchClaims = inputDocument.ClaimStatusBatches.SelectMany(z => z.ClaimStatusBatchClaims).ToList() ?? new List<ClaimStatusBatchClaim>();
                int totalClaims = claimStatusBatchClaims.Count;
                int processedClaims = 0;

                foreach (var claim in claimStatusBatchClaims)
                {
                    try
                    {
                        await ProcessClaimStatusBatchClaim(inputDocument, claim, cancellationToken);

                        // Increment processed claims count
                        processedClaims++;

                        // Calculate progress between 81% and 90%
                        int progress = 81 + (int)Math.Floor(((double)processedClaims / totalClaims) * (95 - 81));

                        // Update progress if it has changed
                       await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", inputDocument.Id, progress);
                    }
                    catch (Exception ex)
                    {
                        var foo = ex.InnerException;
                        // Log exception, continue processing
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public async Task ProcessClaimStatusBatchClaim(InputDocument inputDocument, ClaimStatusBatchClaim claim, CancellationToken cancellationToken)
        {
            ClientCptCode clientCPTCodeExist = await _clientCptCodeRepository.GetByClientId(_clientId, claim.ProcedureCode);
            if (clientCPTCodeExist == null)
            {
                ///If Client CPT code is null or not exist in CLientCPTCode entity 
                ///then we are first check if entry exist in CPT code then create  a fresh entry for ClientCPTCode based on CPTCode details.
                CptCode cptCodeDetail = await _cptCodeRepository.GetByCptCode(claim.ProcedureCode);
                if (cptCodeDetail != null)
                {
                    var cptCode = new ClientCptCode()
                    {
                        ClientId = _clientId,
                        Code = cptCodeDetail.Code,
                        LookupName = cptCodeDetail.Description,
                        Description = cptCodeDetail.Description,
                        ScheduledFee = claim.BilledAmount,
                    };

                    await _unitOfWork.Repository<ClientCptCode>().AddAsync(cptCode);
                    await _unitOfWork.Commit(cancellationToken);
                }
            }

            var isMapped = await _processFeeScheduleMatchedClaimService.ProcessFeeScheduleMatchedClaim(claim);
            if (!isMapped)
            {
                if (claim.ClientInsuranceId != 0 && claim.DateOfServiceFrom.HasValue)
                {
                    int dateOfServiceYear = claim.DateOfServiceFrom.Value.Year;
                    int clientInsuranceId = claim.ClientInsuranceId.Value;
                    var dateOfServiceFrom = claim.DateOfServiceFrom.Value;

                    ///If client CPT code not exist or it is null then we are again fetching procedure code details  based on claim.procedurecode.
                    if (clientCPTCodeExist == null)
                    {
                        clientCPTCodeExist = await _unitOfWork.Repository<ClientCptCode>().Entities.FirstOrDefaultAsync(x => x.Code == claim.ProcedureCode);
                    }

                    if (clientCPTCodeExist != null)
                    {
                        var existingUnMappedFeeScheduleCpt = await _unMappedFeeScheduleCptRepository.GetByCriteria(clientCPTCodeExist.Id, clientInsuranceId, dateOfServiceYear, _clientId);

                        if (existingUnMappedFeeScheduleCpt != null && existingUnMappedFeeScheduleCpt.Id == 0)
                        {
                            var UnMappedFeeScheduleCpt = new UnmappedFeeScheduleCpt()
                            {
                                ClientInsuranceId = clientInsuranceId,
                                DateOfServiceYear = dateOfServiceYear,
                                BilledAmount = claim.BilledAmount ?? 0.00m,
                                ClientId = _clientId,
                                ReferencedDateOfServiceFrom = dateOfServiceFrom,
                                ClientCptCodeId = clientCPTCodeExist.Id
                            };
                            await _unitOfWork.Repository<UnmappedFeeScheduleCpt>().AddAsync(UnMappedFeeScheduleCpt);
                            await _unitOfWork.Commit(cancellationToken);
                        }
                    }
                }
            }
        }
    }
}