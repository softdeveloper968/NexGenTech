using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
	public class ClientAdjustmentCode : AuditableEntity<int>, IDataPipe<int>, IClientRelationship
	{
		public ClientAdjustmentCode()
		{

		}

		public ClientAdjustmentCode(int clientId, string code, string name, string description, AdjustmentTypeEnum adjustmentTypeId, string externalId, DateTime? dfCreatedOn, DateTime? dfLastModifiedOn) 
		{
			ClientId = clientId;
			Code = code;
			Name = name;
			Description = description;
			AdjustmentTypeId = adjustmentTypeId;
			DfExternalId = externalId;
			DfCreatedOn = dfCreatedOn;
			DfLastModifiedOn = dfLastModifiedOn;
		}

		public int ClientId { get; set; }
		public string? Name { get; set; }

		public string? Code { get; set; }

		public string? Description { get; set; }

		public AdjustmentTypeEnum AdjustmentTypeId { get; set; }

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }



		[ForeignKey(nameof(ClientId))]
		public virtual Client Client { get; set; }


		[ForeignKey(nameof(AdjustmentTypeId))]
		public virtual AdjustmentType AdjustmentType { get; set; }
	}
}
