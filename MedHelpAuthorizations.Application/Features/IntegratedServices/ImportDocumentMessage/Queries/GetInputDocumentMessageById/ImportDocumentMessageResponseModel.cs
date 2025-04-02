using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetInputDocumentMessageById
{
    public class ImportDocumentMessageResponseModel
    {
        public int Id { get; set; }
        public int ClaimStatusBatchId { get; set; }
        public int InputDocumentId { get; set; }
        public InputDocumentMessageTypeEnum MessageType { get; set; }
        public string Message { get; set; }

        // Properties to store message information for each type
        public List<MessageInfoViewModel> ErroredMessages { get; set; }
        public List<MessageInfoViewModel> UnmatchedLocationMessages { get; set; }
        public List<MessageInfoViewModel> UnmatchedProviderMessages { get; set; }
		public List<MessageInfoViewModel> FileDuplicates { get; set; }
		public List<MessageInfoViewModel> UnSupplantableDuplicates { get; set; }
		public int? ClaimStatusBatchClaimId { get; set; }
	}

    public class MessageInfoViewModel
    {
        public InputDocumentMessageTypeEnum MessageType { get; set; }
        public string Message { get; set; }
        public int? ClaimStatusBatchClaimId { get; set; }

	}
}
