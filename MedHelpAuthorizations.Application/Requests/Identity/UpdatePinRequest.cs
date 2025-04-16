using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
	public class UpdatePinRequest
	{
		public string UserId { get; set; }
		[Required]
		public string Pin { get; set; }
	}
}
