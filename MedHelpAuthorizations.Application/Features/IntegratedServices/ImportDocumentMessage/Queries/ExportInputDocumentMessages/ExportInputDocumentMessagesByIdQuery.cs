using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetInputDocumentMessageById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.ExportInputDocumentMessages
{
	public class ExportInputDocumentMessagesByIdQuery : IRequest<string>
	{
		public int InputDocumentId { get; set; }
		public string MessageType { get; set; }
		public ExportInputDocumentMessagesByIdQuery() { }
	}

	public class ExportInputDocumentMessagesByIdQueryHandler : IRequestHandler<ExportInputDocumentMessagesByIdQuery, string>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IExcelService _excelService;

		public ExportInputDocumentMessagesByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IExcelService excelService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_excelService = excelService;
		}

		public async Task<string> Handle(ExportInputDocumentMessagesByIdQuery request, CancellationToken cancellationToken)
		{
			try
			{
				// Define the mapping expression for ImportDocumentMessage to ImportDocumentMessageResponseModel
				Expression<Func<Domain.Entities.ImportDocumentMessage, ImportDocumentMessageResponseModel>> expression = e => _mapper.Map<ImportDocumentMessageResponseModel>(e);

				// Create the specification to filter by InputDocumentId
				var inputDocumentMessageSpec = new InputtDocumentMessageByInputDocumentIdSpecification(request.InputDocumentId, request.MessageType);

				// Fetch ImportDocumentMessages and map to ImportDocumentMessageResponseModels
				var importDocumentMessages = await _unitOfWork.Repository<Domain.Entities.ImportDocumentMessage>()
					.Entities
					.Specify(inputDocumentMessageSpec)
					.ToListAsync();

				// Initialize the response model with categorized messages
				var responseModel = new ImportDocumentMessageResponseModel
				{
					ErroredMessages = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.Errored),
					UnmatchedLocationMessages = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.UnmatchedLocation),
					UnmatchedProviderMessages = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.UnmatchedProvider),
					FileDuplicates = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.FileDuplicates),
					UnSupplantableDuplicates = CategorizeMessages(importDocumentMessages, InputDocumentMessageTypeEnum.UnSupplantableDuplicates)
				};

				// Prepare data for exporting
				var exportDetails = new List<IEnumerable<ExportQueryResponse>>();
				var mappingsList = new List<Dictionary<string, Func<ExportQueryResponse, object>>>();
				var sheetNames = new List<string>();

				// Adding each category to the export details, mappings, and sheet names
				if (!string.IsNullOrEmpty(request.MessageType) && request.MessageType == "Error")
				{
					AddSheetData(responseModel.ErroredMessages, exportDetails, mappingsList, sheetNames, "ErroredMessages");
				}
				else if(!string.IsNullOrEmpty(request.MessageType) && request.MessageType == "Missing")
				{
					AddSheetData(responseModel.UnmatchedLocationMessages, exportDetails, mappingsList, sheetNames, "UnmatchedLocationMessages");
					AddSheetData(responseModel.UnmatchedProviderMessages, exportDetails, mappingsList, sheetNames, "UnmatchedProviderMessages");
				}
				else
				{
					AddSheetData(responseModel.FileDuplicates, exportDetails, mappingsList, sheetNames, "FileDuplicates");
					AddSheetData(responseModel.UnSupplantableDuplicates, exportDetails, mappingsList, sheetNames, "UnSupplantableDuplicates");
				}
				
				// Export to Excel with multiple sheets
				var result = await _excelService.ExportMultipleSheetsAsync(exportDetails, mappingsList, sheetNames, boldLastRow: false, applyBoldRowInFirstDataModel: true, applyBoldHeader: false).ConfigureAwait(true);

				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void AddSheetData(List<MessageInfoViewModel> messages, List<IEnumerable<ExportQueryResponse>> exportDetails, List<Dictionary<string, Func<ExportQueryResponse, object>>> mappingsList, List<string> sheetNames, string sheetName)
		{
			var messageResponses = messages.Select(m => new ExportQueryResponse
			{
				MessageType = m.MessageType,
				Message = m.Message,
				ClaimStatusBatchClaimId = m.ClaimStatusBatchClaimId
			}).ToList();

			var mappings = new Dictionary<string, Func<ExportQueryResponse, object>>
			{
				{ "MessageType", x => x.MessageType },
				{ "Message", x => x.Message }
			};

			// Conditionally add the ClaimStatusBatchClaimId column for "UnSupplantableDuplicates" sheet
			if (sheetName == "UnSupplantableDuplicates")
			{
				mappings.Add("ClaimStatusBatchClaimId", x => x.ClaimStatusBatchClaimId);
			}

			exportDetails.Add(messageResponses);
			mappingsList.Add(mappings);
			sheetNames.Add(sheetName);
		}

		private List<MessageInfoViewModel> CategorizeMessages(List<Domain.Entities.ImportDocumentMessage> messages, InputDocumentMessageTypeEnum messageType)
		{
			return messages
				.Where(msg => msg.MessageType == messageType)
				.Select(msg => new MessageInfoViewModel
				{
					MessageType = msg.MessageType,
					Message = msg.Message,
					ClaimStatusBatchClaimId = msg.ClaimStatusBatchClaimId
				})
				.ToList();
		}
	}
}
