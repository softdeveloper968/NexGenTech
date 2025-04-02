using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeDepartments
{
    public class EmployeeDepartmentDto : IRequest<Result<int>>
    {
        public DepartmentEnum DepartmentId { get; set; }

    }
}
