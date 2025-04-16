using FluentValidation;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Authorizations
{
    public class GetAuthorizationByIdResponseValidator : AbstractValidator<GetAuthorizationByIdResponse>
    {
        public GetAuthorizationByIdResponseValidator()
        {
            RuleFor(request => request.AuthNumber)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => $"AuthNumber is required")
                .When(x => x.CompleteDate.HasValue);
            RuleFor(request => request.AuthTypeId)
               .Must(x => x > 0).WithMessage(x => "AuthType is required");
            RuleFor(request => request.EndDate)
                .Must(x => x != null && x != DateTime.MinValue).WithMessage(x => "EndDate is required")
                .When(x => x.CompleteDate.HasValue);
            RuleFor(request => request.Units)
               .Must(x => x > 0).WithMessage(x => "Units must be greater than 0")
               .When(x => x.CompleteDate.HasValue);
            RuleFor(request => request.StartDate)
               .Must(x => x != null && x != DateTime.MinValue).WithMessage(x => "StartDate is required")
               .When(x => x.CompleteDate.HasValue);
        }
             
    }
}
