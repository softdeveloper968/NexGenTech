using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Common.Contracts
{
	public class StgEntity : IStgEntity
	{
		[Key]
		public int StgId { get; set; }
	}
}
