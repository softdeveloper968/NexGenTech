
using MedHelpAuthorizations.Domain.Common.Contracts;
using System;

namespace MedHelpAuthorizations.Domain.Contracts
{
	public interface IDataPipe<TId> : IEntity<TId>
	{
		public string DfExternalId { get; set; }
		public DateTime? DfCreatedOn { get; set; }
		public DateTime? DfLastModifiedOn { get; set; }
	}
}
