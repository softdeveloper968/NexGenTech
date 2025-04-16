using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class GetInputDocumentMessageByMessageTypeSpecification : HeroSpecification<ImportDocumentMessage>
	{
		public GetInputDocumentMessageByMessageTypeSpecification(int inputDocumentId, int messageType)
		{
			Criteria = bc => true;

			Criteria = Criteria.And(bc => bc.InputDocumentId == inputDocumentId);
			if (Enum.IsDefined(typeof(InputDocumentMessageTypeEnum), messageType))
			{
				var enumMessageType = (InputDocumentMessageTypeEnum)messageType;
				Criteria = Criteria.And(bc => bc.MessageType == enumMessageType);
			}
		}
	}
}
