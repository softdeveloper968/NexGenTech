using FluentValidation;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.ClientProvider
{
	public class AddEditProviderCommandValidator : AbstractValidator<AddEditProviderCommand>
	{
		public AddEditProviderCommandValidator(IStringLocalizer<AddEditProviderCommandValidator> localizer)
		{
			RuleFor(request => request.FirstName)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["FirestName is required!"]);
			RuleFor(request => request.LastName)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["LastName is required!"]);
			RuleFor(request => request.Npi)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Npi is required!"])
				.Must(x => !string.IsNullOrWhiteSpace(x) && x.Length == 10).WithMessage(x => localizer["Npi must be 10 characters"]);
			RuleFor(request => request.SpecialtyId)
				.Must(x => x != null).WithMessage(x => localizer["Specialty is required!"]);
		}
	}
}
