using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeRoleClaimStatusExceptionReasonCategories
{
    public class EmployeeRoleClaimStatusExceptionReasonCategoryDto : IRequest<Result<int>>
    {
        public EmployeeRoleEnum EmployeeRoleId { get; set; }
        public ClaimStatusExceptionReasonCategoryEnum ClaimStatusExceptionReasonCategoryId { get; set; }

    }
}
