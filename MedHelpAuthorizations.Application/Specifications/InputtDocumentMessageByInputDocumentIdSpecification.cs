using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class InputtDocumentMessageByInputDocumentIdSpecification : HeroSpecification<ImportDocumentMessage>
	{
		public InputtDocumentMessageByInputDocumentIdSpecification(int inputDocumentId, string messageType)
		{
			Criteria = bc => true;

			Criteria = Criteria.And(bc => bc.InputDocumentId == inputDocumentId);

			if(!string.IsNullOrEmpty(messageType) && messageType == "Error")
			{
				Criteria = Criteria.And(bc => bc.MessageType == InputDocumentMessageTypeEnum.Errored);
			}
			else if(!string.IsNullOrEmpty(messageType) && messageType == "Missing")
			{
				Criteria = Criteria.And(bc => bc.MessageType == InputDocumentMessageTypeEnum.UnmatchedProvider || bc.MessageType == InputDocumentMessageTypeEnum.UnmatchedLocation);
			}
			else if(!string.IsNullOrEmpty(messageType) && messageType == "Repeat")
			{
				Criteria = Criteria.And(bc => bc.MessageType == InputDocumentMessageTypeEnum.FileDuplicates || bc.MessageType == InputDocumentMessageTypeEnum.UnSupplantableDuplicates);
			}
		}
	}
}

