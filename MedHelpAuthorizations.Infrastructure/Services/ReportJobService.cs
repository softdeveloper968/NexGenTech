using AutoMapper;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Application.Features.Reports.CurrentSummary;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Requests.Mail;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Constants.Application;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using RazorEngineCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ReportJobService : IReportJobService
    {
        private readonly IMapper _mapper;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IExcelService _excelService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IHubService _hubService;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ReportJobService> _localizer;
        private readonly IARAgingReportQueryService _reportQueryService;
        private readonly ILogger<ReportJobService> _logger;
        private readonly IClientUserNotificationRepository _clientUserNotificationRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        public ReportJobService(IMapper mapper, IClaimStatusQueryService claimStatusQueryService, IExcelService excelService, IBlobStorageService blobStorageService,
            IHubService hubService, IMailService mailService, IUserService userService, IStringLocalizer<ReportJobService> localizer, IARAgingReportQueryService queryService, ILogger<ReportJobService> logger, IClientUserNotificationRepository clientUserNotificationRepository, IUnitOfWork<int> unitOfWork, ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _mapper = mapper;
            _claimStatusQueryService = claimStatusQueryService;
            _excelService = excelService;
            _blobStorageService = blobStorageService;
            _hubService = hubService;
            _mailService = mailService;
            _userService = userService;
            _localizer = localizer;
            _reportQueryService = queryService;
            _logger = logger;
            _clientUserNotificationRepository = clientUserNotificationRepository;
            _unitOfWork = unitOfWork;
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        #region Finical Summary Report
        /// <summary>
        /// Processes the claim status report and generates an Excel file.
        /// </summary>
        /// <param name="request">The request that contains the parameters for the report.</param>
        /// <returns>A task representing the background process to create the report.</returns>
        public async Task<string> GenerateFinancialSummaryReportAsync(FinicalSummaryExportDetailQuery request, string userId, int clientId, string tenantIdentifier, string conn = null)
        {
            // Generate a dynamic file name (for example, with a timestamp)
            var fileName = request.FileName;
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GenerateFinancialSummaryReportAsync", userId, fileName);
                //// Map the request to the query used to retrieve claim status details
                var detailsQuery = _mapper.Map<ClaimStatusDashboardDetailsQuery>(request);

                //// Retrieve claim status details
                var detailData = await _claimStatusQueryService.GetClaimsStatusDetailsAsync(detailsQuery, clientId, conn);

                // Retrieve export details configuration for claim status details
                var detailExcel = _claimStatusQueryService.GetFinicalSummaryDetailsExcel(request);

                // Create the report options instance
                var options = new ReportCreationOptions
                {
                    Data = detailData,
                    MapperFunc = detailExcel,
                    SheetName = ReportHelper.Financial_Summary,
                    BoldLastRow = false,
                    ApplyBoldRowInFirstDataModel = true,
                    ApplyBoldHeader = false,
                    PasswordString = null,
                    GroupByKeySelector = x => x.PayerName,
                    HasGroupByKeySelector = request.HasGroupByKeySelector,
                    PivotTableConfigurations = new List<ExcelPivotTableConfiguration>
                      {
                        new ExcelPivotTableConfiguration(
                            ReportHelper.Summary, // Sheet name where the pivot table will be created
                            ReportHelper.Summary, // Pivot table name
                            ReportHelper.PerPayer, // Data source name
                            async (pivotTable) => await ConfigureFinancialSummaryPivotTable(pivotTable) // Method to configure the pivot table
                        )
                      },
                    PivotTableSheetOrdering = new Dictionary<string, string>
                      {
                        { ReportHelper.Summary, ReportHelper.Financial_Summary }
                      }
                };

                // Generate the Excel report and save the file
                var reportBase64String = await _excelService.CreateReport(options);

                // Decode the base64 string into a byte array
                byte[] reportBytes = Convert.FromBase64String(reportBase64String);


                // Now you have a valid file path
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes, // Already have the bytes
                    FileName = fileName, // Updated file name
                    UploadType = UploadType.Report,
                    UserId = userId,
                };

                 blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ?  "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                //// Notify the user via email
                await SendReportEmailAsync(userId, blobUrl, fileName, reportBase64String);              
            }
            catch (Exception ex)
            {
                // Notify via SignalR for the front-end to show the download link
                NotifyUserErrorGenerated(fileName, userId, error: "Error occured when file is export", ex.Message, "GenerateFinancialSummaryReportAsync");
                fileStatus = "Error";
                errorMessage = ex.Message;
                throw ex;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(fileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify via SignalR for the front-end to show the download link
                await NotifyUserReportGenerated(fileName, userId, blobUrl);
            }

            return fileName;
        }

        public static async Task<string> GetFileNameFromBlobURLAsync(string blobUrl)
        {
            return await Task.Run(() =>
            {
                // Create a URI object from the URL
                Uri uri = new Uri(blobUrl);

                // Extract the path after the container (beta-documents)
                string fullPath = uri.AbsolutePath;

                // Split the path by '/' and get the parts after the container name
                string[] pathParts = fullPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                // Check if there are at least 2 parts (the container and the file path)
                if (pathParts.Length >= 2)
                {
                    // Join the parts after the container name to get the desired path
                    return string.Join("/", pathParts.Skip(1)); // Skip the first part (container name)
                }

                // If the path does not have the expected format, return the full path
                return fullPath.TrimStart('/');
            });
        }

        /// <summary>
        /// Configures the pivot table for the financial summary report.
        /// </summary>
        /// <param name="pivotTable">The pivot table object to be configured.</param>
        /// <returns>A task representing the asynchronous operation of configuring the pivot table.</returns>
        private static async Task ConfigureFinancialSummaryPivotTable(ExcelPivotTable pivotTable)
        {
            // Add fields to the pivot table
            pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Payer_Name]);
            pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.CPT_Code]);

            var quantityField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Quantity]);
            var allowedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Allowed_Amt]);
            var billedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Billed_Amt]);

            // Specify the format for the fields
            quantityField.Format = "#,##0";
            allowedAmtField.Format = "$#,##0.00";
            billedAmtField.Format = "$#,##0.00";

            // Set DataOnRows as needed
            pivotTable.DataOnRows = false;

            // Collapse all items in each row field:
            foreach (var rowField in pivotTable.RowFields)
            {
                rowField.Items.Refresh();          // Load the pivot items from the source data
                rowField.Items.ShowDetails(false); // Collapse all items (no details shown)
            }

            // As this is asynchronous, return a completed task
            await Task.CompletedTask;
        }
        #endregion

        // Example usage in NotifyUserReportGenerated

        #region Genric Methods
        public async Task NotifyUserReportGenerated(string fileName, string userId, string blobUrl)
        {
            _logger.LogInformation("NotifyUserReportGenerated through UI. File: {FileName}, UserId: {UserId}", fileName, userId);
            await _hubService.SendToUser(ApplicationConstants.SignalR.ReceiveAlertNotification, userId, fileName, blobUrl);
        }

        private void NotifyUserErrorGenerated(string fileName, string userId, string error, string errorMessage, string methodName)
        {
            _logger.LogError("NotifyUserErrorGenerated through UI. File: {FileName} UserId: {userId} Error: {message}", fileName, userId, errorMessage);
            _hubService.SendErrorToUser(ApplicationConstants.SignalR.ReceiveAlertNotification, userId, fileName, error, errorMessage);
        }

        private string GetReportEmailBodyTemplate()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var templatePath = Path.Combine(buildDir, "Templates", "ReportSummaryTemplate.template");
            return File.ReadAllText(templatePath);
        }

        public async Task SendReportEmailAsync(string userId, string reportDownloadUrl, string fileName, string reportBase64String)
        {
            try
            {
                _logger.LogInformation("Sending email to user. File: {FileName} UserId: {UserId}", fileName, userId);
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile(GetReportEmailBodyTemplate());

                // Get the user details using the userId
                var user = await _userService.GetMasterAsync(userId); // Replace with actual method
                string password = string.Empty;

                if (string.IsNullOrEmpty(user?.Data?.Pin))
                {
                    password = "1234N";
                }
                else
                {
                    password = user.Data.Pin;
                }

                // Convert the Base64 string to a byte array
                byte[] byteArray = Convert.FromBase64String(reportBase64String);

                // Create a new Excel package and load the byte array into it
                using (var memoryStream = new MemoryStream(byteArray))
                {
                    using (var package = new ExcelPackage(memoryStream))
                    {
                        byte[] fileArray = await package.GetAsByteArrayAsync(password);

                        // Convert the encrypted byte array to a Base64 string
                        string encryptedFileBase64String = Convert.ToBase64String(fileArray);
                        string cleanedFileName = Regex.Replace(fileName, @"\s*\d{2}-\d{2}-\d{4}\s\d{2}:\d{2}:\d{2}\.xlsx$", "");
                        // Generate the email body using the template, including the download link and file name
                        var emailBody = template.Run(new
                        {
                            FirstName = user.Data.FirstName,
                            ReportDownloadUrl = reportDownloadUrl,
                            FileName = fileName,
                            DecryptionPin = password // Include the PIN in the email (optional)
                        });

                        // Create the email subject
                        var emailSubject = $"Your Report is Ready";

                        // Prepare the email request
                        var emailRequest = new MailRequestWithAttachment
                        {
                            //To = "Mahendra@automatedintegrationtechnologies.com", //For Testing 
                            To = user?.Data?.Email,
                            Subject = emailSubject,
                            Body = emailBody,
                            Base64Content = encryptedFileBase64String,
                            FileName = cleanedFileName
                        };

                        // Send the email using your mail service
                        await _mailService.SendAsync(emailRequest);
                        _logger.LogInformation("File send to user. File: {FileName} UserEmail: {Email}", fileName, user.Data.Email);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (logging or rethrowing)
                throw;
            }
        }
        #endregion

        public async Task<string> GetClaimStatusDetailsSummaryReportAsync(ExportClaimStatusDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileName = string.Empty;
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                _logger.LogInformation("Starting GetClaimStatusDetailsSummaryReportAsync for User: {UserId}, Client: {ClientId}", userId, clientId);

                // Retrieve claim status details
                _logger.LogInformation("Retrieving claim status details");
                // Map the request to the query used to retrieve claim status details
                var detailsQuery = _mapper.Map<ClaimStatusDashboardDetailsQuery>(request);

                // Retrieve claim status details
                var detailData = await _claimStatusQueryService.GetClaimsStatusDetailsAsync(detailsQuery, clientId, conn);
                // Retrieve export details configuration for claim status details
                var detailExcel = _claimStatusQueryService.GetExportDetailsExcel(request);
                var claimStatusExcel = _claimStatusQueryService.GetExportClaimStatusExcel(request);

                if (string.IsNullOrEmpty(request.FileName))
                {
                    fileName = $"ClaimStatusDetails_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                }
                else
                {
                    fileName = request.FileName;
                }

                _logger.LogInformation("File name for export determined as {FileName}", fileName);

                // Check if the request is for exporting all data
                //if (!string.IsNullOrEmpty(request.FlattenedLineItemStatus) &&
                //    (request.FlattenedLineItemStatus.Contains("Export All") || request.FlattenedLineItemStatus.Contains("In-Process")))
                if (!string.IsNullOrEmpty(request.ClaimStatusTypeValue) &&
                   (request.ClaimStatusTypeValue.Contains("Export All")))
                {
                    _logger.LogInformation("Exporting all data for claim status details");
                    var InProcessDetailsFilterFromDetailData = detailData.Where(z => z.ClaimStatusTypeId == null || z.ClaimStatusTransactionId == null).Select(item => new ExportQueryResponse
                    {
                        PatientFirstName = item.PatientFirstName,
                        PatientLastName = item.PatientLastName,
                        PolicyNumber = item.PolicyNumber,
                        ServiceType = item.ServiceType,
                        PayerName = item.PayerName,
                        OfficeClaimNumber = item.OfficeClaimNumber,
                        ProcedureCode = item.ProcedureCode,
                        DateOfServiceFromString = item.DateOfServiceFromString,
                        DateOfServiceToString = item.DateOfServiceToString,
                        BatchNumber = item.BatchNumber,
                        AitClaimReceivedDate = item.AitClaimReceivedDate,
                        AitClaimReceivedTime = item.AitClaimReceivedTime,
                        ClientProviderName = item.ClientProviderName,
                        PaymentType = item.PaymentType,
                        ClientLocationName = item.ClientLocationName,
                        ClientLocationNpi = item.ClientLocationNpi,
                        DateOfBirth = item.DateOfBirth,
                        ClaimBilledOn = item.ClaimBilledOn,
                        BilledAmount = item.BilledAmount,
                        ClaimLevelMd5Hash = item.ClaimLevelMd5Hash,
                    }).ToList();

                    if (string.IsNullOrEmpty(detailsQuery.FlattenedLineItemStatus) || detailsQuery.FlattenedLineItemStatus != "In-Process")
                    {
                        detailsQuery.FlattenedLineItemStatus = "In-Process";
                    }

                    // Retrieve export configuration for in-process details
                    var inProcessExcel = _claimStatusQueryService.GetExportInProcessExcel();

                    // List of sheet names for the export
                    List<string> sheetsName = new() { "Claim Status Details", "InProcess Claims" };

                    // Create a list containing the data to be exported, including summary and detail items.
                    var exportDetails = new List<IEnumerable<ExportQueryResponse>>() { detailData, InProcessDetailsFilterFromDetailData };

                    // Combine the mappings for summary and detail data into a single export report.
                    var mapperList = _claimStatusQueryService.CombineExportDashboardReportDetailModels(claimStatusExcel, inProcessExcel);

                    // Export data to Excel with multiple sheets
                    _logger.LogInformation("Creating multi-sheet Excel report");

                    // Export data to Excel with multiple sheets
                    var exportData = await _excelService.CreateChargesByPayerAndSummarySheets(exportDetails, mapperList, sheetsName,
                        boldLastRow: false, applyBoldRowInFirstDataModel: true, applyBoldHeader: false,
                        groupByKeySelector: x => x.PayerName, hasGroupByKeySelector: request.HasGroupByKeySelector).ConfigureAwait(true);

                    byte[] reportBytes = Convert.FromBase64String(exportData);
                    //var fileBytes = await File.ReadAllBytesAsync(exportData);
                    var uploadRequest = new UploadRequest
                    {
                        Data = reportBytes,  // File content as byte array
                        FileName = fileName,
                        UploadType = UploadType.Report,
                        UserId = userId,
                    };

                    _logger.LogInformation("Uploading report {FileName} to blob storage", fileName);

                     blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                    fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";

                    errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                    _logger.LogInformation("Report {FileName} uploaded successfully. Blob URL: {BlobUrl}", fileName, blobUrl);

                    //var reportBase64String = Convert.ToBase64String(exportData);

                    await SendReportEmailAsync(userId, blobUrl, fileName, exportData);

                    _logger.LogInformation("Report email sent for {FileName} to User: {UserId}", fileName, userId);

                    return fileName; // Return the generated file name
                }

                // If not exporting all data, generate an Excel report for the claim status details
                var exportDataSingle = await _excelService.ExportAsync(detailData,
                    mappers: detailExcel,
                    sheetName: _localizer["Claim Status Report"],
                    workSheetName: _localizer["Export Details"],
                    null,
                    true,
                    groupByKeySelector: x => x.PayerName,
                    hasGroupByKeySelector: request.HasGroupByKeySelector).ConfigureAwait(true);

                byte[] singleFileBytes = Convert.FromBase64String(exportDataSingle);

                var uploadRequestSingle = new UploadRequest
                {
                    Data = singleFileBytes,  // File content as byte array
                    FileName = fileName,
                    UploadType = UploadType.Report,
                    UserId = userId,
                };

                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequestSingle);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                var singlrReportBase64String = Convert.ToBase64String(singleFileBytes);

                await SendReportEmailAsync(userId, blobUrl, fileName, singlrReportBase64String);
                _logger.LogInformation("SignalR notification sent for report {FileName} to User: {UserId}", fileName, userId);             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetClaimStatusDetailsSummaryReportAsync for file: {FileName}", fileName);
                // Notify via SignalR for the front-end to show the download link
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(fileName, userId, error: "Error occured when file is export", ex.Message, "GetClaimStatusDetailsSummaryReportAsync");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(fileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify via SignalR for the front-end to show the download link
                await NotifyUserReportGenerated(fileName, userId, blobUrl);
            }
            return fileName; // Return the generated file name
        }

        public async Task<string> GetInitialClaimStatusReportAsync(ExportInitialClaimStatusDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GetInitialClaimStatusReportAsync", userId, request.FileName);
                var claimStatusData = await _claimStatusQueryService.GetInitialClaimStatusDetailsAsync(request, clientId, conn);
                var claimStatusExcel = _claimStatusQueryService.GetExportInitialClaimStatusReportExcel(request);

                // Generate the Excel report
                var claimStatusFile = await _excelService.CreateClaimStatusReport(claimStatusData, claimStatusExcel, _localizer["Initial Claim Status Report"], null, groupByKeySelector: x => x.PayerName, true).ConfigureAwait(false);

                byte[] reportBytes = Convert.FromBase64String(claimStatusFile);

                // Upload the generated report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes, // Ensure claimStatusFile is a byte array or a path to the file
                    FileName = request.FileName, // Extract file name if it's a file path
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                 blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);
                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the report to Base64 for email attachment (if needed)
                var reportBase64String = Convert.ToBase64String(await File.ReadAllBytesAsync(claimStatusFile)); // Assuming claimStatusFile is a path to the file

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
               
            }
            catch (Exception ex)
            {
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetInitialClaimStatusReportAsync");
                throw ex;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            return blobUrl; // Optionally return the blob URL for further use
        }


        public async Task<string> GetAvgDayToPayReportSummaryReportAsync(ExportAvgDayToPayReportDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GetAvgDayToPayReportSummaryReportAsync", userId, request.FileName);
                // Retrieve claim data
                var claimDataList = await _claimStatusQueryService.GetAverageDaysToPayReportAsync(request,
                    hasAvgDayToPayByProvider: request.HasAvgDayToPayByProvider,
                    connStr: conn,
                    clientId: clientId);

                // Generate summary data based on payer or provider
                var summaryData = GetSummaryExportByPayerORProvider(claimDataList, request.HasAvgDayToPayByProvider);

                // Get the dictionaries for Excel export (details and summary)
                var (detailExcel, summaryExcel) = GetExcelDictionaries(request);
                var exportDetails = new List<IEnumerable<ExportQueryResponse>>() { claimDataList, summaryData };

                // Combine the mappings for summary and detail data into a single export report
                var mapperList = _claimStatusQueryService.CombineExportDashboardReportDetailModels(detailExcel, summaryExcel);

                // Export data to Excel with multiple sheets
                var reportFilePath = await _excelService.ExportMultipleSheetsAsync(exportDetails, mapperList,
                    sheetNames: new List<string> { "Claim Status Details", "Summary" },
                    boldLastRow: false,
                    applyBoldRowInFirstDataModel: true,
                    applyBoldHeader: false
                ).ConfigureAwait(true);

                // Read the exported Excel file as a byte array for further processing
                byte[] reportBytes = Convert.FromBase64String(reportFilePath);

                // Prepare the upload request for Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,  // File content as byte array
                    FileName = request.FileName,  // Extract the file name from the file path
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                // Upload the file to Azure Blob Storage and get the blob URL
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";
                // Convert the file to Base64 string for email attachment

                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetAvgDayToPayReportSummaryReportAsync");
                throw ex;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR that the report is ready
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }
            return blobUrl;  // Optionally return the blob URL for further use
        }

        private (Dictionary<string, Func<ExportQueryResponse, object>> detailExcel, Dictionary<string, Func<ExportQueryResponse, object>> summaryExcel) GetExcelDictionaries(ExportAvgDayToPayReportDetailsQuery request)
        {
            if (request.HasAvgDayToPayByProvider)
            {
                return (
                    _claimStatusQueryService.GetAvgDaysToPayByProviderExportDetailsExcel(request),
                    _claimStatusQueryService.GetExportAvgDayToPayByProviderSummaryExcel()
                );
            }
            else
            {
                return (
                    _claimStatusQueryService.GetAvgDaysToPayExportDetailsExcel(request),
                    _claimStatusQueryService.GetExportAvgDayToPaySummaryExcel()
                );
            }
        }
        private List<ExportQueryResponse> GetSummaryExportByPayerORProvider(List<ExportQueryResponse> claimDataList, bool hasAvgDayToPayByProvider)
        {
            if (hasAvgDayToPayByProvider)
            {

                var summaryData = claimDataList.GroupBy(c => new { c.ProviderId, c.ProviderName }).Select(avg => new ExportQueryResponse
                {
                    ProviderName = avg.Key.ProviderName,
                    AverageDaysToBill = Math.Round(avg.Average(c => c.AvgDaysToBill), 2),
                    AverageDaysToPay = Math.Round(avg.Average(c => c.AvgDaysToPay), 2),
                }).ToList();

                var TotalAverageBilledOnDate = Math.Round((decimal)summaryData.Average(item => item.AverageDaysToBill), 2);
                var TotalAverageServiceToBilledDate = Math.Round((decimal)summaryData.Average(item => item.AverageDaysToPay), 2);

                summaryData.Add(new ExportQueryResponse()
                {
                    ProviderName = "Totals",
                    AverageDaysToBill = (double)TotalAverageBilledOnDate,
                    AverageDaysToPay = (double)TotalAverageServiceToBilledDate,
                });
                return summaryData;
            }
            else
            {
                var summaryData = claimDataList.GroupBy(c => c.PayerName).Select(avg => new ExportQueryResponse
                {
                    PayerName = avg.Key,
                    AverageDaysToBill = Math.Round(avg.Average(c => c.AvgDaysToBill), 2),
                    AverageDaysToPay = Math.Round(avg.Average(c => c.AvgDaysToPay), 2),
                    AverageDaysFromDosTOPay = Math.Round(avg.Average(c => c.AvgDaysfromDOStoPay), 2),
                }).ToList();

                var TotalAverageBilledOnDate = Math.Round((decimal)summaryData.Average(item => item.AverageDaysToBill), 2);
                var TotalAverageServiceToBilledDate = Math.Round((decimal)summaryData.Average(item => item.AverageDaysToPay), 2);
                var TotalDaysFromDosTOPay = Math.Round((decimal)summaryData.Average(item => item.AverageDaysFromDosTOPay), 2);

                summaryData.Add(new ExportQueryResponse()
                {
                    PayerName = "Totals",
                    AverageDaysToBill = (double)TotalAverageBilledOnDate,
                    AverageDaysToPay = (double)TotalAverageServiceToBilledDate,
                    AverageDaysFromDosTOPay = (double)TotalDaysFromDosTOPay,
                });
                return summaryData;
            }
        }

        public async Task<string> GetClaimStatusInProcessDetailsReportAsync(ExportClaimStatusInProcessDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GetClaimStatusInProcessDetailsReportAsync", userId, request.FileName);
                // Map the request to the query object
                var detailsQuery = _mapper.Map<IClaimStatusDashboardDetailsQuery>(request);

                // Fetch the claim status in-process details
                var detailData = await _claimStatusQueryService.GetInProcessDetailsAsync(detailsQuery, clientId: clientId, conn);

                // Export the in-process details to an Excel file
                var reportFilePath = await _excelService.ExportAsync(detailData,
                    workSheetName: _localizer["Unprocessed Worksheet"],
                    mappers: _claimStatusQueryService.MapInProcessDetailsInSheet(),
                    sheetName: _localizer["Claim Status Details"]
                ).ConfigureAwait(true);

                // Read the exported Excel file as a byte array
                byte[] reportBytes = Convert.FromBase64String(reportFilePath);

                // Prepare the upload request for Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,  // File content as byte array
                    FileName = request.FileName,  // Extract the file name from the file path
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                // Upload the file to Azure Blob Storage and get the blob URL
                 blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the file to Base64 string for email attachment
                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetClaimStatusInProcessDetailsReportAsync");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR that the report is ready
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }
            return blobUrl;
        }

        public async Task<string> GetClaimStatusReportAsync(ExportClaimStatusReportQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            _logger.LogInformation("Starting GetClaimStatusReportAsync for UserId: {UserId}, ClientId: {ClientId}", userId, clientId);
            try
            {
                // Map the request to the query used to retrieve claim status details
                var claimStatusQuery = _mapper.Map<ClaimStatusDashboardDetailsQuery>(request);

                _logger.LogInformation("Mapped ExportClaimStatusReportQuery to ClaimStatusDashboardDetailsQuery.");
                // Retrieve claim status details
                var claimStatusData = await _claimStatusQueryService.GetClaimsStatusDetailsAsync(claimStatusQuery, clientId, conn);
                var claimStatusExcel = _claimStatusQueryService.GetExportClaimStatusReportExcel(request);

                _logger.LogInformation("Fetched claim status data successfully.");

                // Generate the Excel report
                var claimStatusFile = await _excelService.CreateClaimStatusReport(claimStatusData, claimStatusExcel, _localizer["Claim Status Report"], null, groupByKeySelector: x => x.PayerName, true).ConfigureAwait(false);

                byte[] reportBytes = Convert.FromBase64String(claimStatusFile);

                // var fileName = $"ClaimStatusDetails_{DateTime.Now:MM-dd-yyyy HH:mm:ss}.xlsx";
                // Upload the generated report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes, // Ensure claimStatusFile is a byte array or a file path
                    FileName = request.FileName, // Extract file name if it's a file path
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);
                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                _logger.LogInformation("Uploaded ClaimStatusReport to Blob Storage: {BlobUrl}", blobUrl);

                // Convert the report to Base64 for email attachment (if needed)
                var reportBase64String = Convert.ToBase64String(reportBytes); // Assuming claimStatusFile is a file path

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
                _logger.LogInformation("Sent ClaimStatusReport via email.");
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetClaimStatusReportAsync");
                throw ex;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
                _logger.LogInformation("Notified user about report generation.");
            }

            // Return the Blob URL (optionally)
            return blobUrl;
        }

        public async Task<string> GetClaimStatusDenialsAsync(ExportClaimStatusDenialsReportQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            _logger.LogInformation("Starting GetClaimStatusDenialsAsync for UserId: {UserId}, ClientId: {ClientId}", userId, clientId);
            try
            {
                // Map the request to the query used to retrieve claim status details
                var claimStatusQuery = _mapper.Map<ClaimStatusDashboardDetailsQuery>(request);
                _logger.LogInformation("Mapped ExportClaimStatusDenialsReportQuery to ClaimStatusDashboardDetailsQuery.");

                // Retrieve claim status details (denials)
                var denialsFilterFromDetailData = await _claimStatusQueryService.GetDenialDetailsAsync(claimStatusQuery, clientId, conn);
                var claimStatusExcel = _claimStatusQueryService.GetExportClaimStatusReportExcel(request);

                _logger.LogInformation("Fetched claim status denials data successfully.");

                // Generate the Excel report for claim status denials
                var claimStatusDenialFile = await _excelService.CreateClaimStatusDenialsReport(denialsFilterFromDetailData, claimStatusExcel, _localizer["Claim Status Denials Report"], null, groupByKeySelector: x => x.PayerName, true).ConfigureAwait(true);

                byte[] reportBytes = Convert.FromBase64String(claimStatusDenialFile);
                var fileName = request.FileName;

                _logger.LogInformation("Generated Excel report for ClaimStatusDenials.");

                // Upload the generated report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,
                    FileName = fileName,
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL
                 blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";
                _logger.LogInformation("Uploaded ClaimStatusDenialsReport to Blob Storage: {BlobUrl}", blobUrl);

                // Convert the report to Base64 for email attachment (if needed)
                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, fileName, reportBase64String);
                _logger.LogInformation("Sent ClaimStatusDenialsReport via email.");
             
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetClaimStatusDenialsAsync");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
                _logger.LogInformation("Notified user about denial report generation.");
            }

            // Return the Blob URL (optionally)
            return blobUrl;
        }

        public async Task<string> GetInitialClaimStatusDenialsReportAsync(ExportInitialClaimStatusDenialsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GetInitialClaimStatusDenialsReportAsync", userId, request.FileName);
                // Retrieve the claim status data for denials
                var claimStatusData = await _claimStatusQueryService.GetInitialClaimStatusDenialDetailsAsync(request, clientId, conn);

                // Prepare the claim status Excel mapping
                var claimStatusExcel = _claimStatusQueryService.GetExportInitialClaimStatusReportExcel(request);

                // Generate the Excel report for claim status denials
                var claimStatusFile = await _excelService.CreateClaimStatusDenialsReport(claimStatusData, claimStatusExcel, _localizer["Initial Claim Status Denials Report"], null, groupByKeySelector: x => x.PayerName, true).ConfigureAwait(false);

                var fileName = request.FileName;


                byte[] reportBytes = Convert.FromBase64String(claimStatusFile);

                // Upload the generated report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes, // Assuming claimStatusFile is a byte array or a file path
                    FileName = fileName, // Use the dynamically generated file name
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the report to Base64 for email attachment (if needed)
                var reportBase64String = Convert.ToBase64String(reportBytes); // Assuming claimStatusFile is a path to the file

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, fileName, reportBase64String);

            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetInitialClaimStatusDenialsReportAsync");
                throw ex;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);
                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL (optionally)
            return blobUrl;
        }

        public async Task<string> GetExportClaimStatusDenialDetailsReportAsync(ExportClaimStatusDenialDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GetExportClaimStatusDenialDetailsReportAsync", userId, request.FileName);
                // Map the request to the query used to retrieve claim status details
                var detailsQuery = _mapper.Map<ClaimStatusDashboardDenialsDetailQuery>(request);

                // Retrieve claim status details
                var detailData = await _claimStatusQueryService.GetDenialDetailsAsync(detailsQuery, clientId, conn);

                // Retrieve export details configuration for claim status details
                var detailDataExcel = _claimStatusQueryService.GetExcelDenialReport();

                // Create the report options instance
                var options = new ReportCreationOptions
                {
                    Data = detailData,
                    MapperFunc = detailDataExcel,
                    SheetName = ReportHelper.Denial_Summary,
                    BoldLastRow = false,
                    ApplyBoldRowInFirstDataModel = true,
                    ApplyBoldHeader = false,
                    PasswordString = null,
                    GroupByKeySelector = x => x.ExceptionReasonCategory,
                    HasGroupByKeySelector = request.HasGroupByKeySelector,
                    PivotTableConfigurations = new List<ExcelPivotTableConfiguration>
                    {
                        new ExcelPivotTableConfiguration(
                            ReportHelper.Summary, // Sheet name where the pivot table will be created
                            ReportHelper.Summary, // Pivot table name
                            ReportHelper.PerStatus, // Data source name
                            async (pivotTable) => await ConfigureDenialSummaryPivotTable(pivotTable) // Method to configure the pivot table
                        )
                    },
                };

                // Generate the Excel report
                var reportFile = await _excelService.CreateReport(options);
                byte[] reportBytes = Convert.FromBase64String(reportFile);

                // Upload the generated report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,
                    FileName = request.FileName,
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);

            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetExportClaimStatusDenialDetailsReportAsync");
                throw ex;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL (optionally)
            return blobUrl;
        }


        /// <summary>
        /// Configures the pivot table for the denial summary report.
        /// </summary>
        /// <param name="pivotTable">The pivot table object to be configured.</param>
        /// <returns>A task representing the asynchronous operation of configuring the pivot table.</returns>
        private static async Task ConfigureDenialSummaryPivotTable(ExcelPivotTable pivotTable)
        {
            // Add fields to the pivot table
            pivotTable.RowFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Exception_Reason]);

            var quantityField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Quantity]);
            var billedAmtField = pivotTable.DataFields.Add(pivotTable.Fields[StoredProcedureColumnsHelper.Billed_Amt]);

            // Specify the format for the fields
            quantityField.Format = "#,##0";
            billedAmtField.Format = "$#,##0.00";

            // Set DataOnRows as needed
            pivotTable.DataOnRows = false;

            // As this is asynchronous, return a completed task
            await Task.CompletedTask;
        }

        public async Task<string> GetInitialClaimStatusDenialDetailsReportAsync(ExportInitialClaimStatusDenialDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GetInitialClaimStatusDenialDetailsReportAsync", userId, request.FileName);
                // Retrieve claim status denial details
                var detailData = await _claimStatusQueryService.GetInitialClaimStatusDenialDetailsAsync(request, clientId, conn);

                // Generate the Excel report with specified columns and mappings
                var exportData = await _excelService.ExportAsync(detailData,
                    workSheetName: _localizer["Initial Claim Denials Detail Worksheet"],
                    mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
                    {
                     { _localizer["Exception Reason"], item => item.ExceptionReason },
                        { _localizer["Last Name"], item => item.PatientLastName },
                        { _localizer["First Name"], item => item.PatientFirstName },
                        { _localizer["Provider"], item => item.ClientProviderName },
                        { _localizer["DOB"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfBirth?.ToString("MM/dd/yyyy")) },
                        { _localizer["Policy Number"], item => item.PolicyNumber },
                        { _localizer["Service Type"], item => item.ServiceType },
                        { _localizer["Payer Name"], item => item.PayerName },
                        { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                        { _localizer["Ins Claim #"], item => item.PayerClaimNumber },
                        { _localizer["Ins Lineitem Control #"], item => item.PayerLineItemControlNumber },
                        { _localizer["DOS From"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceFrom?.ToString("MM/dd/yyyy")) },
                        { _localizer["DOS To"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DateOfServiceTo ?.ToString("MM/dd/yyyy"))  },
                        { _localizer["CPT Code"], item => item.ProcedureCode },
                        { _localizer["Billed On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.ClaimBilledOn ?.ToString("MM/dd/yyyy"))  },
                        { _localizer["Billed Amt"], item =>  _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US")))},
                        { _localizer["Lineitem Status"], item => item.ClaimLineItemStatus },
                        //{ _localizer["Reported Lineitem Status"], item => item.ClaimLineItemStatusValue },
                        { _localizer["Exception Category"], item => item.ExceptionReasonCategory },
                        { _localizer["Exception Remark"], item => item.ExceptionRemark },
                        { _localizer["Remark Code"], item => item.RemarkCode },
                        { _localizer["Remark Description"], item => item.RemarkDescription },
                        { _localizer["Reason Code"], item => item.ReasonCode },
                        { _localizer["Reason Description"], item => item.ReasonDescription },
                        { _localizer["Deductible Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.DeductibleAmount ?.ToString("C", new CultureInfo("en-US"))) },
                        { _localizer["Copay Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType,  item.CopayAmount?.ToString("C", new CultureInfo("en-US"))) },
                        { _localizer["Coinsurance Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CoinsuranceAmount ?.ToString("C", new CultureInfo("en-US") )) },
                        { _localizer["Penality Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PenalityAmount ?.ToString("C", new CultureInfo("en-US"))) },
                        { _localizer["Lineitem Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.LineItemPaidAmount ?.ToString("C", new CultureInfo("en-US"))) },
                        { _localizer["Check #"], item => item.CheckNumber },
                        { _localizer["Check Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CheckDate ?.ToString("MM/dd/yyyy"))  },
                        { _localizer["Check Paid Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.CheckPaidAmount ?.ToString("C", new CultureInfo("en-US"))) },
                        { _localizer["Eligibility Ins"], item => item.EligibilityInsurance },
                        { _localizer["Eligibility Policy #"], item => item.EligibilityPolicyNumber },
                        { _localizer["Eligibility From Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.EligibilityFromDate ?.ToString("MM/dd/yyyy"))  },
                        { _localizer["Eligibility Status"], item => item.EligibilityStatus },
                        { _localizer["VerifiedMemberId"], item => item.VerifiedMemberId },
                        { _localizer["CobLastVerified"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CobLastVerified ?.ToString("MM/dd/yyyy")) },
                        { _localizer["LastActiveEligibleDateRange"], item => item.LastActiveEligibleDateRange },
                        { _localizer["PrimaryPayer"], item => item.PrimaryPayer },
                        { _localizer["PrimaryPolicyNumber"], item => item.PrimaryPolicyNumber },
                        { _localizer["PartA_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityFrom ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartA_EligibilityTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_EligibilityTo ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartA_DeductibleFrom "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleFrom ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartA_DeductibleTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartA_DeductibleTo ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartA_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartA_RemainingDeductible ?.ToString("C", new CultureInfo("en-US")))},
                        { _localizer["PartB_EligibilityFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityFrom ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartB_EligibilityTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_EligibilityTo ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartB_DeductibleFrom "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleFrom ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartB_DeductibleTo "], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PartB_DeductibleTo ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PartB_RemainingDeductible"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PartB_RemainingDeductible ?.ToString("C", new CultureInfo("en-US")))},
                        { _localizer["OtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearFrom ?.ToString("MM/dd/yyyy")) },
                        { _localizer["OtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.OtCapYearTo ?.ToString("MM/dd/yyyy")) },
                        { _localizer["OtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.OtCapUsedAmount ?.ToString("C", new CultureInfo("en-US")))},
                        { _localizer["PtCapYearFrom"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearFrom ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PtCapYearTo"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PtCapYearTo ?.ToString("MM/dd/yyyy")) },
                        { _localizer["PtCapUsedAmount"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.PtCapUsedAmount ?.ToString("C", new CultureInfo("en-US"))) },
                        { _localizer["AIT Batch #"], item => item.BatchNumber },
                        { _localizer["AIT Received Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.AitClaimReceivedDate ?? null) },
                        { _localizer["AIT Received Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.AitClaimReceivedTime ?? null) },
                        { _localizer["AIT Transaction Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.TransactionDate ?? null) },
                        { _localizer["AIT Transaction Time"], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.TransactionTime ?? null) },
                        {_localizer[StoredProcedureColumnsHelper.Last_Checked_Date], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastCheckedDate ?? null) },
                        {_localizer[StoredProcedureColumnsHelper.Last_Checked_Time], item => _excelService.AddTypePrefix(ExportHelper.TimeType, item.LastCheckedTime ?? null) },
                        { _localizer["Client Location Name"], item => item.ClientLocationName },
                        { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                        //{ _localizer["Last History Created On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.LastHistoryCreatedOn ?.ToString("MM/dd/yyyy"))}, //EN-127
                        { _localizer["ClaimStatusBatchId"], item => item.ClaimStatusBatchClaimId }  //EN-127
                            },
                    sheetName: _localizer["Claim Denials Detail"]
                );

                byte[] reportBytes = Convert.FromBase64String(exportData);
                // Upload the report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,
                    FileName = request.FileName,
                    UploadType = UploadType.Report,
                    UserId = userId
                };

                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message , "GetInitialClaimStatusDenialDetailsReportAsync");
                throw ex;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL
            return blobUrl;
        }

        public async Task<string> GetCustomPaymentAndProcedureReportAsync(ExportCustomPaymentAndProcedureCodeQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("GetCustomPaymentAndProcedureReportAsync", userId, request.FileName);
                // Map the request to the query used to retrieve claim status details
                var detailsQuery = _mapper.Map<ClaimStatusDashboardDetailsQuery>(request);

                // Retrieve claim status details
                var detailData = await _claimStatusQueryService.GetClaimsStatusDetailsAsync(detailsQuery, clientId: clientId, conn);

                // Retrieve export details configuration for claim status details
                var detailExcel = _claimStatusQueryService.GetPaymentAndProcedureCodeExportDetailsExcel(request);

                // List of sheet names for the export
                string sheetsName = "Claim Status Report";

                // Export data to Excel with multiple sheets
                var claimStatusFile = await _excelService.CreateInsuranceReimbursementAndPivotTableSheet(detailData, detailExcel, sheetsName, sheetsName).ConfigureAwait(true);

                // Convert the generated report to byte array
                byte[] reportBytes = Convert.FromBase64String(claimStatusFile);

                // Upload the generated report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes, // Byte array of the report
                    FileName = request.FileName, // Dynamically generated file name
                    UploadType = UploadType.Report, // Specify the upload type as Report
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the report to Base64 for email attachment
                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "GetCustomPaymentAndProcedureReportAsync");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL of the uploaded report
            return blobUrl;
        }

        public async Task<string> ExportCashValueForRevenueAsync(ExportCashValueForRevenueQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("ExportCashValueForRevenueAsync", userId, request.FileName);
                //Updated AA-331
                // Create a query to retrieve the cash value for revenue data
                GetCashValueForRevenueByDayQuery query = new()
                {
                    FilterBy = request.FilterBy,
                    FilterForDays = request.FilterForDays,
                    ClientProviderIds = request.ClientProviderIds,
                    ClientLocationIds = request.ClientLocationIds,
                };

                var response = await _claimStatusQueryService.GetCashValueForRevenueByDayAsync(query, clientId, conn);
                var detailResponse = await _claimStatusQueryService.GetCashValueForRevenueDetails(query, clientId, conn);

                // Filter the data based on the selected filter criteria from the UI
                var selectedItems = new List<ExportQueryResponse>();
                var selectedDetailItems = new List<ExportQueryResponse>();

                // If SelectedDate is null or empty, export all data; otherwise, export only for the selected date.
                if (string.IsNullOrEmpty(request.SelectedDate))
                {
                    selectedItems = GroupAndSummarize(response, request.FilterBy);
                    selectedDetailItems = detailResponse.Select(z => new ExportQueryResponse
                    {
                        CashValue = z.CashValue,
                        PayerName = z.PayerName,
                        ServiceDate = z.ServiceDate,
                        BilledDate = z.BilledDate,
                        PatientFirstName = z.PatientFirstName,
                        PatientLastName = z.PatientLastName,
                        ProcedureCode = z.ProcedureCode,
                        Location = z.Location,
                        Provider = z.Provider
                    }).ToList();
                }
                else
                {
                    selectedItems = GroupAndSummarize(response.Where(x => x.ServiceDate == request.SelectedDate).ToList(), request.FilterBy);
                    selectedDetailItems = detailResponse.Where(x => x.ServiceDate == request.SelectedDate)
                                        .Select(z => new ExportQueryResponse
                                        {
                                            CashValue = z.CashValue,
                                            PayerName = z.PayerName,
                                            ServiceDate = z.ServiceDate,
                                            BilledDate = z.BilledDate,
                                            PatientFirstName = z.PatientFirstName,
                                            PatientLastName = z.PatientLastName,
                                            ProcedureCode = z.ProcedureCode,
                                            Location = z.Location,
                                            Provider = z.Provider
                                        }).ToList();
                }

                // Get the Excel report mappings for summary and detail data.
                var excelReport = _claimStatusQueryService.GetExcelCashValueForRevenue();
                var excelReportDetails = _claimStatusQueryService.GetExcelCashValueForRevenueDetails();

                // Create a list of sheet names for the Excel export.
                List<string> sheetsName = new() { "Export Summary", "Export Details" };

                // Combine the mappings for summary and detail data.
                var exportDetails = new List<IEnumerable<ExportQueryResponse>>() { selectedItems, selectedDetailItems };
                var mapperList = _claimStatusQueryService.CombineTwoExportReportDetailModels(excelReport, excelReportDetails);

                // Export the data to multiple sheets in an Excel file.
                var cashValueReport = await _excelService.ExportMultipleSheetsAsync(exportDetails, mapperList, sheetsName, boldLastRow: false, applyBoldRowInFirstDataModel: true, applyBoldHeader: false).ConfigureAwait(true);

                // Convert the generated Excel report to a byte array.
                byte[] reportBytes = Convert.FromBase64String(cashValueReport); // Assuming cashValueReport is Base64 encoded.

                // Upload the report to Azure Blob Storage.
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes, // Byte array of the report.
                    FileName = request.FileName, // Dynamic file name.
                    UploadType = UploadType.Report, // Specify the upload type as Report.
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL.
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);

                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the report to Base64 for email attachment (if needed).
                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email.
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "ExportCashValueForRevenueAsync");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic ways
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR.
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL of the uploaded report.
            return blobUrl;
        }

        public static List<ExportQueryResponse> GroupAndSummarize(List<GetCashValueForRevenueByDayResponse> source, string filterBy)
        {
            switch (filterBy)
            {
                case "ServiceDate":
                    return source.GroupBy(x => new { x.ServiceDate, x.PayerName })
                                .Select(x => new ExportQueryResponse()
                                {
                                    ClaimCount = x.Sum(y => y.ClaimCount),
                                    CashValue = x.Sum(y => y.CashValue),
                                    RevenueTotals = x.Sum(y => y.RevenueTotals),
                                    PayerName = x.Key.PayerName,
                                    ServiceDate = x.Key.ServiceDate,
                                }).ToList();

                case "BilledDate":
                    return source.GroupBy(x => new { x.BilledDate, x.PayerName })
                                .Select(x => new ExportQueryResponse()
                                {
                                    ClaimCount = x.Sum(y => y.ClaimCount),
                                    CashValue = x.Sum(y => y.CashValue),
                                    RevenueTotals = x.Sum(y => y.RevenueTotals),
                                    PayerName = x.Key.PayerName,
                                    BilledDate = x.Key.BilledDate,
                                }).ToList();

                default: return new();
            }

        }

        public async Task<string> ARAgingReportExportDetailsAsync(ARAgingReportExportDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("ARAgingReportExportDetailsAsync", userId, request.FileName);
                int filterDayGroupby = 30;
                string filterReportBy = "BilledOnDate";

                // Determine filter criteria based on preset filter type
                if (request.PresetFilterType != DashboardPresets.PresetFilterTypeEnum.BilledOnDate)
                {
                    filterReportBy = string.Empty;
                }

                // Retrieve the day group based on the enum
                _reportQueryService.GetFilterDayGroupBy(request.ARAgingReportDayGroupEnum, out filterDayGroupby);

                // Retrieve aging report details
                var detailData = await _reportQueryService.GetARAgingReportExportDetailsAsync(request, filterDayGroupby, filterReportBy, conn);

                // Get the Excel report mappings for the details
                var excelReportDetails = _reportQueryService.GetExcelARAgingReportDetails();

                // Export the data to an Excel sheet
                var agingReport = await _excelService.ExportAsync(detailData, workSheetName: _localizer["Export Details"], mappers: excelReportDetails, sheetName: _localizer["ARAging Report Details"]).ConfigureAwait(true);

                // Convert the generated Excel report to a byte array
                byte[] reportBytes = Convert.FromBase64String(agingReport); // Assuming the report is Base64 encoded


                // Upload the report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,  // Byte array of the report
                    FileName = request.FileName, // Dynamic file name
                    UploadType = UploadType.Report, // Specify the upload type as Report
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);
                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the report to Base64 for email attachment (if needed)
                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "ARAgingReportExportDetailsAsync");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL of the uploaded report
            return blobUrl;
        }

        public async Task<string> ARAgingReportExportSummaryAsync(ARAgingReportExportSummaryQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("ARAgingReportExportSummaryAsync", userId, request.FileName);
                int filterDayGroupby = 30;
                string filterReportBy = "BilledOnDate";

                // Determine the filter report based on preset filter type
                if (request.PresetFilterType != DashboardPresets.PresetFilterTypeEnum.BilledOnDate)
                {
                    filterReportBy = string.Empty;
                }

                // Retrieve day group based on enum
                _reportQueryService.GetFilterDayGroupBy(request.ARAgingReportDayGroupEnum, out filterDayGroupby);

                // Create an export query for details and summary
                var exportQuery = new ARAgingReportExportDetailsQuery
                {
                    ARAgingReportDayGroupEnum = request.ARAgingReportDayGroupEnum,
                    ClientInsuranceIds = request.ClientInsuranceIds,
                    ClientLocationIds = request.ClientLocationIds,
                    PresetFilterType = request.PresetFilterType,
                    ClientProviderIds = request.ClientProviderIds,
                    FileName = request.FileName,
                };

                // Fetching the detailed and summary data
                var detailData = await _reportQueryService.GetARAgingReportExportDetailsAsync(exportQuery, filterDayGroupby, filterReportBy, conn);
                var summaryData = await _reportQueryService.GetARAgingReportExportSummaryAsync(exportQuery, filterDayGroupby, filterReportBy, conn);

                // If Exception Reason Category is null, combine category with 'Other' type
                summaryData = _reportQueryService.UpdateExportDetails(summaryData);

                // Add grand total in the summary sheet
                summaryData.Add(new ExportQueryResponse()
                {
                    ExceptionReasonCategory = "Grand Total",
                    ExceptionCount = summaryData.Sum(z => z.ExceptionCount),
                    SumBilledAmount = summaryData.Sum(z => z.SumBilledAmount)
                });

                // Get Excel report details and summary mappings
                var excelReportDetails = _reportQueryService.GetExcelARAgingReportDetails();
                var excelSummaryReportDetails = _reportQueryService.GetExcelARAgingReportSummary();

                // List of sheet names for export
                List<string> sheetsName = new() { "Export Summary", "Export Details" };

                // Data to be exported (summary and details)
                var exportDetails = new List<IEnumerable<ExportQueryResponse>>() { summaryData, detailData };

                // Combine the mapping for both summary and details
                var mapperList = _reportQueryService.CombineTwoExportReportDetailModels(excelSummaryReportDetails, excelReportDetails);

                // Export data to Excel with multiple sheets
                var agingReport = await _excelService.ExportMultipleSheetsAsync(exportDetails, mapperList, sheetsName, boldLastRow: true, applyBoldRowInFirstDataModel: true, applyBoldHeader: false).ConfigureAwait(true);

                // Convert the generated Excel report to a byte array
                byte[] reportBytes = Convert.FromBase64String(agingReport); // Assuming the report is Base64 encoded

                // Upload the report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,  // Byte array of the report
                    FileName = request.FileName, // Dynamic file name
                    UploadType = UploadType.Report, // Specify the upload type as Report
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);
                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the report to Base64 for email attachment (if needed)
                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);

            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "ARAgingReportExportSummaryAsync");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL of the uploaded report
            return blobUrl;

        }

        public async Task<string> ExportInitialClaimStatusInProcessDetails(ExportInitialClaimStatusInProcessDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier)
        {
            string fileStatus = string.Empty;
            string errorMessage = string.Empty;
            string blobUrl = string.Empty;
            try
            {
                LogMethodStart("ExportInitialClaimStatusInProcessDetails", userId, request.FileName);
                var detailData = await _claimStatusQueryService.GetInitialInProcessDetailsAsync(request, clientId, conn);
                var exportData = await _excelService.ExportAsync(detailData, workSheetName: _localizer["Unprocessed Worksheet"], mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
                {
                    { _localizer["Last Name"], item => item.PatientLastName },
                    { _localizer["First Name"], item => item.PatientFirstName },
                    { _localizer["DOB"], item => item.DateOfBirth },
                    { _localizer["Provider"], item => item.ClientProviderName },
                    { _localizer["Payer Name"], item => item.PayerName },
                    { _localizer["Service Type"], item => item.ServiceType },
                    { _localizer["Policy Number"], item => item.PolicyNumber },
                    { _localizer["Office Claim #"], item => item.OfficeClaimNumber },
                    { _localizer["DOS From"], item => item.DateOfServiceFrom },
                    { _localizer["DOS To"], item => item.DateOfServiceTo },
                    { _localizer["CPT Code"], item => item.ProcedureCode },
                    { _localizer["Billed On"], item => item.ClaimBilledOn },
                    { _localizer["Billed Amt"], item => _excelService.AddTypePrefix(ExportHelper.CurrencyType, item.BilledAmount.ToString("C", new CultureInfo("en-US")))},
                    { _localizer["AIT Batch #"], item => item.BatchNumber },
                    { _localizer["AIT Received Date"], item => item.AitClaimReceivedDate },
                    { _localizer["AIT Received Time"], item => item.AitClaimReceivedTime },
                    { _localizer["Client Location Name"], item => item.ClientLocationName },
                    { _localizer["Client Location Npi"], item => item.ClientLocationNpi },
                    { _localizer["PaymentType"], item => item.PaymentType }, //AA-324
			    }, sheetName: _localizer["Initial In-Process Claim Details"]).ConfigureAwait(true);

                // Convert the generated Excel report to a byte array
                byte[] reportBytes = Convert.FromBase64String(exportData); // Assuming the report is Base64 encoded

                // Upload the report to Azure Blob Storage
                var uploadRequest = new UploadRequest
                {
                    Data = reportBytes,  // Byte array of the report
                    FileName = request.FileName, // Dynamic file name
                    UploadType = UploadType.Report, // Specify the upload type as Report
                    UserId = userId
                };

                // Upload to Blob Storage and get the Blob URL
                blobUrl = await _blobStorageService.UploadToBlobStorageAndReturnUrlAsync(uploadRequest);
                fileStatus = !string.IsNullOrWhiteSpace(blobUrl) ? "Completed" : "Error";
                errorMessage = !string.IsNullOrWhiteSpace(blobUrl) ? string.Empty : "Error creating report on blob storage.";

                // Convert the report to Base64 for email attachment (if needed)
                var reportBase64String = Convert.ToBase64String(reportBytes);

                // Send the report via email
                await SendReportEmailAsync(userId, blobUrl, request.FileName, reportBase64String);  
            }
            catch (Exception ex)
            {
                fileStatus = "Error";
                errorMessage = ex.Message;
                NotifyUserErrorGenerated(request.FileName, userId, error: "Error occured when file is export", ex.Message, "ExportInitialClaimStatusInProcessDetails");
                throw;
            }
            finally
            {
                // Commit the notification update in a generic way
                await CommitClientUserNotificationAsync(request.FileName, fileStatus, blobUrl, errorMessage, userId, clientId, tenantIdentifier);

                // Notify the user via SignalR
                await NotifyUserReportGenerated(request.FileName, userId, blobUrl);
            }

            // Return the Blob URL of the uploaded report
            return blobUrl;
        }

        private void LogMethodStart(string methodName, string userId, string fileName)
        {
            _logger.LogInformation("Starting Report Job service {MethodName} for UserId: {UserId}, FileName: {FileName}", methodName, userId, fileName);
        }

        /// <summary>
        /// Commits the client user notification update.
        /// This generic helper encapsulates the creation and update of a notification object.
        /// </summary>
        private async Task CommitClientUserNotificationAsync(string fileName, string fileStatus, string blobUrl, string errorMessage, string userId, int clientId, string tenantIdentifier)
        {
            var _clientUserNotificationRepository = await _tenantRepositoryFactory.GetAsync<IClientUserNotificationRepository>(tenantIdentifier);
            var clientNotificationData = await _clientUserNotificationRepository.GetNotificationByFileNameAsync(fileName, clientId);

            if (clientNotificationData != null &&  (clientNotificationData.FileName != "Completed" || clientNotificationData.FileName != "Error"))
            {
                clientNotificationData.FileStatus = fileStatus;
                clientNotificationData.ErrorMessage = errorMessage;
                clientNotificationData.FileUrl = await GetFileNameFromBlobURLAsync(blobUrl);
                await _clientUserNotificationRepository.UpdateAsync(clientNotificationData);
            }
            
            _logger.LogInformation("Status Update to complete. File: {FileName}, UserId: {UserId}", fileName, userId);
        }

    }
}
