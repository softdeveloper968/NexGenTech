using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Requests.Mail;
using MedHelpAuthorizations.Shared.Constants.BlobStorage;
using MedHelpAuthorizations.Shared.Extensions;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Shared.Requests;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RazorEngineCore;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
	{
		private readonly BlobServiceClient _blobServiceClient;
		private readonly string _containerName;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly ILogger<ReportJobService> _logger;
        public BlobStorageService(BlobServiceClient blobServiceClient, ICurrentUserService currentUserService, IUserService userService, IMailService mailService, ILogger<ReportJobService> logger)
		{
			_blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
			_containerName = BlobStorageConfig.ContainerName;
            _currentUser = currentUserService;
            _userService = userService;
            _mailService = mailService;
            _logger = logger;
        }

		public async Task UploadBlobAsync(string blobName, Stream content, string contentType)
		{
			if (string.IsNullOrEmpty(_containerName)) throw new ArgumentNullException(nameof(_containerName));
			if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException(nameof(blobName));
			if (content == null) throw new ArgumentNullException(nameof(content));
			if (string.IsNullOrEmpty(contentType)) throw new ArgumentNullException(nameof(contentType));

			var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
			await containerClient.CreateIfNotExistsAsync();
			var blobClient = containerClient.GetBlobClient(blobName);
			await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });
		}

        /// <summary>
        /// Downloads a blob from Azure Blob Storage as a stream.
        /// </summary>
        /// <param name="blobName">The name of the folder name and file name e.g.(Report/FinancialSummary_20241004_110737.xlsx) to download.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the 
        /// <see cref="BlobDownloadInfo"/> for the downloaded blob.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the container name or blob name is not provided.</exception>
        /// <exception cref="Exception">Thrown if an error occurs during the download process.</exception>
        public async Task<BlobDownloadInfo> DownloadBlobAsStreamAsync(string blobName)
		{
			try
			{
                if (string.IsNullOrEmpty(_containerName)) throw new ArgumentNullException(nameof(_containerName));
                if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException(nameof(blobName));

                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(blobName);
                var response = await blobClient.DownloadAsync();
                return response.Value;
            }
			catch (Exception ex)
			{

				throw ex;
			}
		
		}
		public async Task<BlobStorageFileDownloadResponse> DownloadBlobAsByteArrayAsync(string blobName)
		{
			var response = await DownloadBlobAsStreamAsync(blobName);
			if (response is not null)
			{
				// Read the stream into a byte array
				using MemoryStream ms = new();
				await response.Content.CopyToAsync(ms);
				var byteArray = ms.ToArray();
				var fileDetails = response.Details;

				return new BlobStorageFileDownloadResponse
				{
					ContentType = "application/octet-stream",
					FileContent = byteArray,
					FileName = Uri.UnescapeDataString(BlobStorageHelper.GetFileNameFromBlobURL(blobName))
				};
			}
			return null;
		}

		public async Task DeleteBlobAsync(string blobName)
		{
			if (string.IsNullOrEmpty(_containerName)) throw new ArgumentNullException(nameof(_containerName));
			if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException(nameof(blobName));
			//string extractBlobName = BlobStorageHelper.ExtractBlobName(blobName, _containerName);

			var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
			var blobClient = containerClient.GetBlobClient(blobName);
			await blobClient.DeleteIfExistsAsync();
		}

		public async Task UploadFolderAsync(string folderPath)
		{
			if (string.IsNullOrEmpty(_containerName)) throw new ArgumentNullException(nameof(_containerName));
			if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath));
			if (!Directory.Exists(folderPath)) throw new DirectoryNotFoundException($"The folder path '{folderPath}' does not exist.");

			var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
			foreach (var file in files)
			{
				var relativePath = Path.GetRelativePath(folderPath, file);
				using var stream = File.OpenRead(file);
				var contentType = GetContentType(file);
				await UploadBlobAsync(relativePath, stream, contentType);
			}
		}

		private string GetContentType(string filePath)
		{
			var extension = Path.GetExtension(filePath).ToLowerInvariant();
			return extension switch
			{
				".csv" => "text/csv",
				".txt" => "text/plain",
				".jpg" => "image/jpeg",
				".png" => "image/png",
				".pdf" => "application/pdf",
				_ => "application/octet-stream",
			};
		}


		public async Task<string> UploadToBlobStorageAsync(UploadRequest request)
		{
			if (request.Data == null) return string.Empty;

			var streamData = new MemoryStream(request.Data);
			if (streamData.Length > 0)
			{
				try
				{

					var folder = request.UploadType.GetDescription();
					var fileName = request.FileName.Trim('"');
					var blobName = Uri.EscapeUriString(Path.Combine(folder, fileName).Replace("\\", "/"));//.Trim().Replace(" ", "_");
					if (string.IsNullOrEmpty(_containerName)) throw new ArgumentNullException(nameof(_containerName));

					BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
					await containerClient.CreateIfNotExistsAsync();

					// Check if blob exists
					BlobClient blobClient = containerClient.GetBlobClient(blobName);
					if (await blobClient.ExistsAsync())
					{
						blobName = BlobStorageHelper.NextAvailableFilename(blobName);
						blobClient = containerClient.GetBlobClient(blobName);
					}

					streamData.Position = 0; // Reset the stream position
					await blobClient.UploadAsync(streamData, true);

					var blobURL = blobClient.Uri.ToString();
					string extractBlobName = $"/{BlobStorageHelper.ExtractBlobName(blobURL, _containerName)}";
					return Uri.UnescapeDataString(extractBlobName);
				}
				catch (Exception e)
				{

					throw;
				}
			}
			else
			{
				return string.Empty;
			}
		}

		public async Task<string> GetImageURL(string blobName)
		{
			if (string.IsNullOrEmpty(_containerName)) throw new ArgumentNullException(nameof(_containerName));
			if (string.IsNullOrEmpty(blobName)) throw new ArgumentNullException(nameof(blobName));

			var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
			var blobClient = containerClient.GetBlobClient(blobName);

			if (await blobClient.ExistsAsync())
			{
				var downloadInfo = await blobClient.DownloadAsync();

				using (var ms = new MemoryStream())
				{
					await downloadInfo.Value.Content.CopyToAsync(ms);
					var imageBytes = ms.ToArray();
					var base64String = Convert.ToBase64String(imageBytes);
					return $"data:image/png;base64,{base64String}";
				}
			}
			return string.Empty;
		}


        /// <summary>
        /// Uploads to BlobUrl under {UploadType}/{ClientCode}/{DateTime.Now}
        /// </summary>
        /// <param name="request"></param>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<string> UploadToBlobStorageAsync(UploadRequest request, string clientCode = null) //EN-791
        {
            if (request.Data == null) return string.Empty;

            var streamData = new MemoryStream(request.Data);
            if (streamData.Length > 0)
            {
                try
                {
                    // Define folder and file name
                    if (!string.IsNullOrEmpty(request.FileName))
                    {
                        // Create folder structure
                        var folder = !string.IsNullOrWhiteSpace(clientCode)
                            ? $"{request.UploadType.GetDescription()}/{clientCode}/{DateTime.Now:yyyy-MM-dd}"
                            : $"{request.UploadType.GetDescription()}/{DateTime.Now:yyyy-MM-dd}";

                        var fileName = request.FileName.Trim('"');
                        var blobName = Path.Combine(folder, fileName).Replace("\\", "/").Trim().Replace(" ", "_");

                        if (string.IsNullOrEmpty(_containerName))
                            throw new ArgumentNullException(nameof(_containerName));

                        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                        await containerClient.CreateIfNotExistsAsync();

                        BlobClient blobClient = containerClient.GetBlobClient(blobName);

                        if (await blobClient.ExistsAsync())
                        {
                            blobName = BlobStorageHelper.NextAvailableFilename(blobName);
                            blobClient = containerClient.GetBlobClient(blobName);
                        }

                        // Reset stream position and upload
                        streamData.Position = 0;
                        await blobClient.UploadAsync(streamData, true);

                        var blobURL = blobClient.Uri.ToString();
                        string extractBlobName = $"/{BlobStorageHelper.ExtractBlobName(blobURL, _containerName)}";
                        return Uri.UnescapeDataString(extractBlobName);                        
                    }
                    return null;
                }
                catch (Exception e)
                {
                    // Handle any errors
                    throw new ApplicationException("An error occurred while uploading the file to Blob Storage.", e);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Uploads the provided data to Azure Blob Storage and returns the URL of the uploaded blob.
        /// </summary>
        /// <param name="request">The upload request containing the data to upload, the file name, and the upload type.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the URL of the uploaded blob, 
        /// or an empty string if the upload request is invalid.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the container name is not provided.</exception>
        /// <exception cref="Exception">Thrown if an error occurs during the upload process.</exception>
        public async Task<string> UploadToBlobStorageAndReturnUrlAsync(UploadRequest request)
        {
            _logger.LogInformation("Uploading File to Blob. FileName: {FileName} UserId: {UserId}", request.FileName, request.UserId);
            if (request.Data == null) return string.Empty;

            var streamData = new MemoryStream(request.Data);
            if (streamData.Length > 0)
            {
                try
                {
					// Define folder and file name
					if (!string.IsNullOrEmpty(request.FileName))
					{
						var folder = request.UploadType.GetDescription();
						var fileName = request.FileName.Trim('"');
						var blobName = Uri.EscapeUriString(Path.Combine(folder, fileName).Replace("\\", "/"));

						// Ensure container name is provided
						if (string.IsNullOrEmpty(_containerName)) throw new ArgumentNullException(nameof(_containerName));

						// Get Blob container client
						BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
						await containerClient.CreateIfNotExistsAsync();

						// Get Blob client
						BlobClient blobClient = containerClient.GetBlobClient(blobName);

						// If blob exists, generate a new filename to avoid overwriting
						if (await blobClient.ExistsAsync())
						{
							blobName = BlobStorageHelper.NextAvailableFilename(blobName);
							blobClient = containerClient.GetBlobClient(blobName);
						}

						// Reset stream position and upload
						streamData.Position = 0;
						await blobClient.UploadAsync(streamData, true);

                        _logger.LogInformation("File uploaded to Blob. FileName: {FileName} UserId: {UserId}", request.FileName, request.UserId);

                        // Return the full Blob URL
                        return blobClient.Uri.ToString();
					}
					return null;
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Error occured while Uploading File to Blob. FileName: {FileName} UserId: {UserId}", request.FileName, request.UserId);
                    // Handle any errors
                    throw;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> UploadToBlobStorageAndSendEmailToUserAsync(UploadRequest request)
        {
            var fileUrl = await UploadToBlobStorageAndReturnUrlAsync(request);
            if (!string.IsNullOrEmpty(fileUrl))
            {
                var reportBase64String = Convert.ToBase64String(request.Data);

				await SendReportEmailAsync(_currentUser.UserId, fileUrl, request.FileName, reportBase64String);

                // Returning the file URL after email is sent
                return fileUrl;
            }
            return string.Empty;
        }

        public async Task SendReportEmailAsync(string userId, string reportDownloadUrl, string fileName, string reportBase64String)
        {
            try
            {
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

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

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
                            //To = "Naveen@automatedintegrationtechnologies.com", //For Testing 
                            To = user?.Data?.Email,
                            Subject = emailSubject,
                            Body = emailBody,
                            Base64Content = encryptedFileBase64String,
                            FileName = cleanedFileName
                        };

                        // Send the email using your mail service
                        await _mailService.SendAsync(emailRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (logging or rethrowing)
                throw;
            }
        }

        private string GetReportEmailBodyTemplate()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var templatePath = Path.Combine(buildDir, "Templates", "ReportSummaryTemplate.template");
            return File.ReadAllText(templatePath);
        }
    }

}
