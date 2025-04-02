using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.UserEmployee.Commands.AddEdit
{
    public class AddEditUserEmployeeCommand : IRequest<Result<string>>
    {
    }
}
