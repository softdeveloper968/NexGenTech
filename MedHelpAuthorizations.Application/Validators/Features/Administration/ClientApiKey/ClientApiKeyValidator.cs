using FluentValidation;
using MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.ClientApiKey
{
	public class ClientApiKeyValidator : AbstractValidator<AddEditClientApiKeyCommand>
	{
		public ClientApiKeyValidator()
		{
			RuleFor(requests => requests.ApiKey)
					   .Must(x => x != null).WithMessage("ApiKey Is Required!");

			RuleFor(requests => requests.ApiUrl)
					   .Must(x => x != null).WithMessage("ApiUrl Is Required!");
			RuleFor(requests => requests.ApiIntegrationId)
				.GreaterThan(0).WithMessage("Api Is Required");
		}
		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<AddEditClientApiKeyCommand>.CreateWithOptions((AddEditClientApiKeyCommand)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
