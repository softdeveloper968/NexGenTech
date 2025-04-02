using Azure.Storage.Blobs.Models;
using CsvHelper;
using Hangfire.Dashboard;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Requests;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Commands.AddEdit
{
    public class AddEditImportProcessInputDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public InputDocumentTypeEnum? InputDocumentTypeId { get; set; }
        public int? AuthTypeId { get; set; }
        public UploadRequest UploadRequest { get; set; }
        public int ClientId { get; set; }
        public string Url { get; set; } = string.Empty;
    }
    public class AddEditImportProcessInputDocumentCommandHandler : IRequestHandler<AddEditImportProcessInputDocumentCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IInputDataService _inputDataService;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IInputDocumentRepository _inputDocumentRepository;
        private readonly IUploadService _uploadService;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IClientFeeScheduleService _clientFeeScheduleService; //AA-315
        private readonly IUnMappedFeeScheduleCptRepository _unMappedFeeScheduleCptRepository;  //EN-155
        private readonly IClientCptCodeRepository _clientCptCodeRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IHubService _hubService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditImportProcessInputDocumentCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IInputDataService inputDataService,
            IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository,
            IInputDocumentRepository inputDocumentRepository,
            ICurrentUserService currentUserService,
            IUploadService uploadService,
            IMediator mediator,
            IClientFeeScheduleService clientFeeScheduleService,
            IUnMappedFeeScheduleCptRepository unMappedFeeScheduleCptRepository, //EN-155
            IClientCptCodeRepository clientCptCodeRepository,
            IBlobStorageService blobStorageService, IHubService hubService)
        {
            _unitOfWork = unitOfWork;
            _inputDataService = inputDataService;
            _mediator = mediator;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _uploadService = uploadService;
            _inputDocumentRepository = inputDocumentRepository;
            _currentUserService = currentUserService;
            _clientFeeScheduleService = clientFeeScheduleService;
            _unMappedFeeScheduleCptRepository = unMappedFeeScheduleCptRepository;
            _clientCptCodeRepository = clientCptCodeRepository;
            _blobStorageService = blobStorageService;
            _hubService = hubService;
        }

        public async Task<Result<int>> Handle(AddEditImportProcessInputDocumentCommand command, CancellationToken cancellationToken)
        {
        //TODO: Put in metrics to know how long it takes to run through this method
             var uploadRequest = command.UploadRequest ?? new UploadRequest(); ;
            var inputDocument = await _unitOfWork.Repository<InputDocument>().GetByIdAsync(command.Id);
            int attemptedImportCount = 0;
            int actualImportCount = 0;
            var requestMessages = new List<string>();

            if (inputDocument == null)
            {
                throw new Exception($"InputDocument with Id {command.Id} was not found.");
            }
            // Process Input document (make batches, etc.)
            try
            {
                switch (command.InputDocumentTypeId)
                {

                    case InputDocumentTypeEnum.ClaimStatusInput:
                        IList<ClaimStatusBatchClaim> claimStatusBatchClaims = new List<ClaimStatusBatchClaim>();
                        IList<ClaimStatusBatchClaimModel> erroredBatchClaims = new List<ClaimStatusBatchClaimModel>();
                        IList<ClaimStatusBatchClaimModel> unmatchedLocationBatchClaims = new List<ClaimStatusBatchClaimModel>();
                        IList<ClaimStatusBatchClaimModel> unmatchedProviderBatchClaims = new List<ClaimStatusBatchClaimModel>();
                        IList<ClaimStatusBatchClaimModel> filesDuplicates = new List<ClaimStatusBatchClaimModel>();
                        IList<ClaimStatusBatchClaimModel> unsupplantableDuplicates = new List<ClaimStatusBatchClaimModel>();
                        ClaimStatusBatch claimStatusBatch = null;
                        HashSet<ClaimStatusBatchClaimModel> batchClaimModels = null;

						#region Old File Download method

						////Retry to upload file 
						//if (!string.IsNullOrEmpty(command.Url))
						//{
						//    // Delete the Messages
						//    Expression<Func<Domain.Entities.ImportDocumentMessage, bool>> cfeExpression = x => x.InputDocumentId == inputDocument.Id;

						//    _unitOfWork.Repository<Domain.Entities.ImportDocumentMessage>().ExecuteDelete(cfeExpression);

						//    var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

						//    var task = GetFileNameAsync(command.Url);

						//    // Await the result to get the file name
						//    string fileName = await task;
						//    //// Combine the root directory with the relative path to the infrastructure folder
						//    //string infrastructureDirectory = Path.Combine(buildDir, "..", "MedHelpAuthorizations.Infrastructure");
						//    string binDirectory = AppDomain.CurrentDomain.BaseDirectory;
						//    // Navigate to the Infrastructure project directory
						//    string infrastructureDirectory = Path.GetFullPath(Path.Combine(binDirectory, "..", "..", ".."));
						//    // Combine the infrastructure directory with the SeedData folder and the file name
						//    string csvFilePath = Path.Combine(infrastructureDirectory, "Files\\IntegratedServices\\InputDocuments", fileName);
						//    string fullPath = Path.Combine(infrastructureDirectory + $"\\ {command.Url}");

						//    if (File.Exists(csvFilePath))
						//    {
						//        using (var reader = new StreamReader(csvFilePath))
						//        {
						//            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
						//            {
						//                batchClaimModels = csv.GetRecords<ClaimStatusBatchClaimModel>().ToHashSet() ?? new HashSet<ClaimStatusBatchClaimModel>();

						//                // Initialize a MemoryStream to hold the CSV data
						//                using (var memoryStream = new MemoryStream())
						//                {
						//                    // Create a CsvWriter and pass the memory stream
						//                    using (var writer = new StreamWriter(memoryStream))
						//                    using (var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
						//                    {
						//                        // Write the records to the memory stream
						//                        csvWriter.WriteRecords(batchClaimModels);

						//                        // Flush the writer to ensure all data is written to the memory stream
						//                        writer.Flush();

						//                        // Convert the memory stream to a byte array
						//                        uploadRequest.FileName = fileName;
						//                        uploadRequest.UploadType = Shared.Enums.UploadType.InputDocument;
						//                        uploadRequest.Extension = ".csv";
						//                        uploadRequest.Data = memoryStream.ToArray();
						//                    }
						//                }
						//            }
						//        }
						//    }
						//}

						#endregion

						#region Handle URL with blobstorage

						if (!string.IsNullOrEmpty(command.Url))
                        {
                            // Delete the Messages
                            Expression<Func<Domain.Entities.ImportDocumentMessage, bool>> cfeExpression = x => x.InputDocumentId == inputDocument.Id;
                            _unitOfWork.Repository<Domain.Entities.ImportDocumentMessage>().ExecuteDelete(cfeExpression);

                            // Get the file name from the URL
                            string fileName = await GetFileNameAsync(command.Url);
                            // Download the blob content as BlobDownloadInfo
                            BlobDownloadInfo blobDownloadInfo = await _blobStorageService.DownloadBlobAsStreamAsync(fileName);

                            // Convert BlobDownloadInfo to MemoryStream
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                await blobDownloadInfo.Content.CopyToAsync(memoryStream, cancellationToken);
                                memoryStream.Position = 0; // Reset the stream position

                                using (StreamReader reader = new StreamReader(memoryStream))
                                {
                                    using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                    {
                                        batchClaimModels = csv.GetRecords<ClaimStatusBatchClaimModel>().ToHashSet() ?? new HashSet<ClaimStatusBatchClaimModel>();

                                        // Initialize a MemoryStream to hold the CSV data
                                        using (var outputMemoryStream = new MemoryStream())
                                        {
                                            // Create a CsvWriter and pass the memory stream
                                            using (var writer = new StreamWriter(outputMemoryStream))
                                            {
                                                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                                                {
                                                    // Write the records to the memory stream
                                                    csvWriter.WriteRecords(batchClaimModels);

                                                    // Flush the writer to ensure all data is written to the memory stream
                                                    writer.Flush();

                                                    // Convert the memory stream to a byte array
                                                    uploadRequest.FileName = fileName;
                                                    uploadRequest.UploadType = Shared.Enums.UploadType.InputDocument;
                                                    uploadRequest.Extension = ".csv";
                                                    uploadRequest.Data = outputMemoryStream.ToArray();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", command.Id, 3);

                        Tuple<IList<ClaimStatusBatchClaim>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, IList<ClaimStatusBatchClaimModel>, int> batchClaimsObjects
                         = await _inputDataService.DeserializeClaimStatusInputDataAsync(claimStatusBatchClaims, erroredBatchClaims, unmatchedLocationBatchClaims, unmatchedProviderBatchClaims, filesDuplicates, unsupplantableDuplicates, attemptedImportCount,
                         (int)inputDocument.ClientInsuranceId, uploadRequest?.Data, uploadRequest?.Extension, command.Id);

                        claimStatusBatchClaims = batchClaimsObjects.Item1;
                        erroredBatchClaims = batchClaimsObjects.Item2;
                        unmatchedLocationBatchClaims = batchClaimsObjects.Item3;
                        unmatchedProviderBatchClaims = batchClaimsObjects.Item4;
                        filesDuplicates = batchClaimsObjects.Item5;
                        unsupplantableDuplicates = batchClaimsObjects.Item6;

                        attemptedImportCount = batchClaimsObjects.Item7;

						actualImportCount = claimStatusBatchClaims.Count();

                        // Check if there's an existing claim status batch related to the input document
                        ClaimStatusBatch claimStatusBatchData = await _unitOfWork.Repository<ClaimStatusBatch>().Entities.FirstOrDefaultAsync(x => x.InputDocumentId == command.Id) ?? null;
                        if (claimStatusBatchData != null)
                        {
                            int previousProgress = 65; // Starting at 65% for this section
                            claimStatusBatch = claimStatusBatchData;
                            inputDocument.ClaimStatusBatches.Add(claimStatusBatchData);

                            // Add claim status batch claims to each existing claim status batch
                            int totalBatchCount = inputDocument.ClaimStatusBatches.Count;
                            int batchIndex = 0;

                            // Add claim status batch claims to each existing claim status batch
                            foreach (var batch in inputDocument.ClaimStatusBatches)
                            {
                                batchIndex++;
                                foreach (var claimStatusBatchClaim in claimStatusBatchClaims)
                                {
                                    batch.ClaimStatusBatchClaims.Add(claimStatusBatchClaim);
                                }

                                // Update progress as we add claims to each batch
                                int progress = 65 + (int)Math.Floor(((double)batchIndex / totalBatchCount) * (70 - 65)); // Update from 65% to 70%
                                if (progress > previousProgress)
                                {
                                    previousProgress = progress;
                                    await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", command.Id, progress);
                                }
                            }

                            await _unitOfWork.Repository<InputDocument>().UpdateAsync(inputDocument);
                            await _unitOfWork.Commit(cancellationToken);
                        }
                        else if (claimStatusBatchClaims != null)
                        {

                            claimStatusBatch = await _inputDataService.ProcessClaimStatusBatchClaims(inputDocument, claimStatusBatchClaims, command.AuthTypeId, cancellationToken);
                            inputDocument.ClaimStatusBatches.Add(claimStatusBatch);
                            await _unitOfWork.Repository<InputDocument>().UpdateAsync(inputDocument);
                            await _unitOfWork.Commit(cancellationToken);

                            await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", command.Id, 75);

                        };


                        // Create a list to store ImportDocumentMessages

                        List<Domain.Entities.ImportDocumentMessage> importDocumentMessages = await _inputDataService.CreateImportDocumentMessages(erroredBatchClaims.ToList(), unmatchedProviderBatchClaims.ToList(), unmatchedLocationBatchClaims.ToList(), claimStatusBatch.Id, inputDocument.Id, filesDuplicates.ToList(), unsupplantableDuplicates.ToList());


                        // Add importDocumentMessages to the database context
                        if (importDocumentMessages != null && importDocumentMessages.Any())
                        {
                            _unitOfWork.Repository<Domain.Entities.ImportDocumentMessage>().AddRange(importDocumentMessages);
                            await _unitOfWork.Commit(cancellationToken);
                        }


                        await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", command.Id, 80);

                        //AA-315
                        // Check if there are ClaimStatusBatches in the input document
                        if (inputDocument?.ClaimStatusBatches?.Any() ?? false)
                        {
                            await _inputDataService.ProcessClaimStatusBatches(inputDocument, cancellationToken);
                        }

                        await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", command.Id, 95);
                        break;


                    case InputDocumentTypeEnum.ClaimStatusWriteOff:
                        var claimStatusBatchClaimsTransactionCreated = new List<ClaimStatusBatchClaim>();
                        var claimStatusBatchClaimsNotFound = new List<ClaimStatusWriteOffs>();
                        var claimStatusWriteOffs = new List<ClaimStatusBatchClaim>();
                        var claimStatusWriteOffsCollection = await _inputDataService.DeserializeClaimStatusWriteOffDataAsync(uploadRequest?.Data, uploadRequest?.Extension);

                        if (claimStatusWriteOffsCollection.Count > 0)
                        {
                            //It should only return the deserialized modal
                            foreach (var bc in claimStatusWriteOffsCollection)
                            {
                                ClaimStatusBatchClaimModel claimStatusModel = new ClaimStatusBatchClaimModel()
                                {
                                    PatientFirstName = bc.PatientFirstName,
                                    PatientLastName = bc.PatientLastName,
                                    DateOfBirth = bc.DateOfBirth,
                                };
                                int? patientId = await _inputDataService.GetPatientId(claimStatusModel, cancellationToken);

                                //Compute same hash identifier that is in the DB.
                                //Look up claim by hash. 
                                //Determine if a an active claim can be supplanted by a new claim and new pipeline be worked. 
                                //var claimMd5Hash = HashHelpers.CreateClaimSql0xMD5(bc.PatientLastName, bc.PatientFirstName, bc.DateOfBirth, bc.ClaimNumber, bc.ProcedureCode, bc.Modifiers, bc.DateOfServiceFrom.Value);
                                var claimMd5Hash = HashHelpers.CreateClaimSql0xMD5(patientId, bc.ProcedureCode?.Substring(0, 5), bc.Modifiers, bc.DateOfServiceFrom.Value);
                                var activeBatchClaim = await _claimStatusBatchClaimsRepository.GetActiveByEntryMd5HashAsync(claimMd5Hash);
                                if (activeBatchClaim == null)
                                {
                                    claimStatusBatchClaimsNotFound.Add(bc);
                                }
                                else
                                {
                                    claimStatusWriteOffs.Add(activeBatchClaim);
                                }
                                continue;
                            }
                            foreach (var claimStatus in claimStatusWriteOffs)
                            {
                                if (claimStatus.ClaimStatusTransactionId != null && claimStatus.ClaimStatusTransactionId != 0)
                                {
                                    claimStatus.ClaimStatusTransaction.ClaimLineItemStatusId = ClaimLineItemStatusEnum.Writeoff;
                                    if (claimStatus.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId != null && claimStatus.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId != 0)
                                    {
                                        claimStatus.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.PreviousClaimLineItemStatusId = claimStatus.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatusId;
                                        claimStatus.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatusId = ClaimLineItemStatusEnum.Writeoff;
                                        await _unitOfWork.Repository<ClaimStatusTransactionLineItemStatusChangẹ>().UpdateAsync(claimStatus.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ);
                                    }
                                    else
                                    {
                                        var claimStatusTransactionLineItemStatusChangẹ = new ClaimStatusTransactionLineItemStatusChangẹ(_clientId, null, ClaimLineItemStatusEnum.Writeoff, DbOperationEnum.Insert);
                                        await _unitOfWork.Repository<ClaimStatusTransactionLineItemStatusChangẹ>().AddAsync(claimStatusTransactionLineItemStatusChangẹ);
                                        await _unitOfWork.Commit(cancellationToken);
                                    }
                                    await _unitOfWork.Repository<ClaimStatusTransaction>().UpdateAsync(claimStatus.ClaimStatusTransaction);
                                    await _unitOfWork.Commit(cancellationToken);
                                }
                                else
                                {
                                    claimStatusBatchClaimsTransactionCreated.Add(claimStatus);
                                    var ClaimStatusTransaction = new ClaimStatusTransaction(_clientId, claimStatus.Id, DateTime.UtcNow, DateTime.UtcNow, ClaimStatusEnum.None);
                                    await _unitOfWork.Repository<ClaimStatusTransaction>().AddAsync(ClaimStatusTransaction);
                                    await _unitOfWork.Commit(cancellationToken);
                                }
                            }

                        };

                        foreach (var bc in claimStatusBatchClaimsTransactionCreated)
                        {
                            requestMessages.Add($"Transaction created - Claim#: {bc.ClaimNumber};  Patient: {bc.PatientLastName}, {bc.PatientFirstName};  DOB: {bc.DateOfBirth.Value.ToString("MM/dd/yyyy")};  Policy: {bc.PolicyNumber};  Procedure: {bc.ProcedureCode};  DOS: {bc.DateOfServiceFrom.Value.ToString("MM/dd/yyyy")}");
                        }
                        foreach (var bc in claimStatusBatchClaimsNotFound)
                        {
                            requestMessages.Add($"Claims not found - Claim#: {bc.ClaimNumber};  Patient: {bc.PatientLastName}, {bc.PatientFirstName};  DOB: {bc.DateOfBirth.Value.ToString("MM/dd/yyyy")};  Procedure: {bc.ProcedureCode};  DOS: {bc.DateOfServiceFrom.Value.ToString("MM/dd/yyyy")}");
                        }

                        break;

                    default:
                        break;

                }
                if (inputDocument != null)
                {
                    inputDocument.ImportStatus = ImportStatusEnum.Completed;
                    inputDocument.AttemptedImportCount = attemptedImportCount;
                    inputDocument.ActualImportCount = actualImportCount;
                    await _unitOfWork.Repository<InputDocument>().UpdateAsync(inputDocument);
                    await _unitOfWork.Commit(cancellationToken);
                }

                await _hubService.RefreshTable("RefreshTable", command.Id);
                await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", command.Id, 99);

                // Add importDocumentMessages to the database context
                return Result<int>.Success(command.Id, requestMessages);
            }
            catch (Exception ex)
            {
                requestMessages.Add(ex.Message);
                if (inputDocument != null)
                {
                    inputDocument.ImportStatus = ImportStatusEnum.Error;
                    inputDocument.ErrorMessage = ex.Message;
                    inputDocument.AttemptedImportCount = attemptedImportCount;
                    inputDocument.ActualImportCount = actualImportCount;
                    await _unitOfWork.Repository<InputDocument>().UpdateAsync(inputDocument);
                    await _unitOfWork.Commit(cancellationToken);
                }
                //throw;
                return Result<int>.Fail(requestMessages);
            }
            finally
            {
                await _unitOfWork.Commit(cancellationToken);
                await _hubService.SendFileUploadPercentageToClient("ReceivePercentage", command.Id, 100);
            }
        }

        static async Task<string> GetFileNameAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL cannot be null or empty.");

            // Find the index of the last occurrence of the backslash character
            int lastIndex = url.LastIndexOf('\\');

            // Get the substring starting from the character after the last backslash
            return await Task.FromResult(url.Substring(lastIndex + 1));
        }
    }
}
