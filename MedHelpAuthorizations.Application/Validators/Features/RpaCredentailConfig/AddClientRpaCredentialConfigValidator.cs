using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Validators.Features.RpaCredentailConfig
{
	public class AddClientRpaCredentialConfigValidator : AbstractValidator<CreateClientRpaCredentialConfigCommand>
	{
		public AddClientRpaCredentialConfigValidator()
		{
			RuleFor(requests => requests.Username)
					   .Must(username => !string.IsNullOrEmpty(username))
							.WithMessage("Username Is Required!");

			RuleFor(requests => requests.Password)
					   .Must(password => !string.IsNullOrEmpty(password)).WithMessage("Password amount Is Required!");

			RuleFor(requests => requests.ReportFailureToEmail)
					  .Must(reportFailureToEmail => !string.IsNullOrEmpty(reportFailureToEmail)).WithMessage("ReportFailureToEmail Is Required!")
					   .EmailAddress().WithMessage("ReportFailureToEmail should be a valid email address!");


			//RuleFor(requests => requests.OtpForwardFromEmail)
			//          .Must(x => x != null).WithMessage("OtpForwardFromEmail Is Required!");

		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<CreateClientRpaCredentialConfigCommand>.CreateWithOptions((CreateClientRpaCredentialConfigCommand)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
