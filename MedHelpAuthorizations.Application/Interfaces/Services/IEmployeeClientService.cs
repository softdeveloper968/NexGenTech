using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IEmployeeClientService : IService
    {
        public Task<List<EmployeeClientDto>> GetEmployeeClientsByRolesDepartmentsLevels(int clientId, List<EmployeeLevelEnum> employeeLevels, List<DepartmentEnum> departments, List<EmployeeRoleEnum> employeeRoleIds, IEmployeeClientRepository employeeClientRepository);
    }
}
