using System;
using FluentValidation;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetById;

namespace MedHelpAuthorizations.Application.Validators.Features.Patients
{
    public class GetPatientByIdResponseValidator : AbstractValidator<GetPatientByIdResponse>
    {
        public GetPatientByIdResponseValidator()
        {

            //RuleFor(request => request.AccountNumber)
            //    .Must(x => !string.IsNullOrWhiteSpace(x))
            //    .WithMessage(x => $"Account Number is required");
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage(x => "Last Name is required");
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage(x => "First Name is required");
            RuleFor(request => request.DateOfBirth)
                .Must(x => x != null && x != DateTime.MinValue)
                .WithMessage(x => "Date of Birth is required");
            RuleFor(request => request.DecryptedSocialSecurityNumber) //AA-218
                .Must(x => x.ToString().Length == 9)
                .When(x => x.DecryptedSocialSecurityNumber != null)
                .WithMessage(x => "Social Security Number must be 9 numeric characters in length");
            //RuleFor(request => request.ClientInsuranceId)
            //    .Must(x => x != null && x != 0)
            //    .WithMessage(x => "Insurance Name is required");
            //RuleFor(request => request.InsurancePolicyNumber)
            //    .Must(x => !string.IsNullOrWhiteSpace(x))
            //    .WithMessage(x => "Insurance Policy is required");
            //RuleFor(request => request.InsuranceGroupNumber)
            //    .Must(x => !string.IsNullOrWhiteSpace(x))
            //    .WithMessage(x => "Insurance Group is required");
        }
    }
}
