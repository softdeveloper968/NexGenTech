using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class EmployeeClientService : IEmployeeClientService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        public EmployeeClientService(IMapper mapper, IUnitOfWork<int> unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
    }

    public async Task<List<EmployeeClientDto>> GetEmployeeClientsByRolesDepartmentsLevels(int clientId, List<EmployeeLevelEnum> employeeLevelIds, List<DepartmentEnum> departmentIds, List<EmployeeRoleEnum> employeeRoleIds, IEmployeeClientRepository employeeClientRepository)
        {
            try
            {
                if (employeeClientRepository == null)
                {
                    return null;
                }

                IQueryable<EmployeeClient> query = employeeClientRepository.Entities.Include(x => x.Employee)
                                                    .Include("EmployeeClientInsurances.ClientInsurance")
                                                    .Include("EmployeeClientLocations.ClientLocation")
                                                    .Include("AssignedClientEmployeeRoles.EmployeeRole.EmployeeRoleClaimStatusExceptionReasonCategories")
                                                    .Include("AssignedClientEmployeeRoles.EmployeeRole.EmployeeRoleDepartments")
                                                    .Include("EmployeeClientAlphaSplits.AlphaSplit");

                if (clientId == 0)
                    throw new ArgumentOutOfRangeException("Parameter (int clientId) value must be > 0");

                query = query.Where(x => x.ClientId == clientId);

                if (employeeLevelIds.Any())
                    query = query.Where(x => x.AssignedClientEmployeeRoles.Any(cer => employeeLevelIds.Contains(cer.EmployeeRole.EmployeeLevel)));

                if (departmentIds.Any())
                    query = query.Where(x => x.AssignedClientEmployeeRoles.Any(cer => cer.EmployeeRole.EmployeeRoleDepartments.Any(cer => departmentIds.Contains(cer.DepartmentId))));

                if (employeeRoleIds.Any())
                    query = query.Where(x => x.AssignedClientEmployeeRoles.Any(cer => employeeRoleIds.Contains(cer.EmployeeRoleId)));

                var employeeClients = await query.Select(x => _mapper.Map<EmployeeClientDto>(x)).ToListAsync();

                foreach (var empClient in employeeClients) //AA-230
                {
                    var user = await _userManager.FindByIdAsync(empClient.Employee.UserId);
                    if (user != null)
                    {
                        empClient.Employee.User = user;
                    }
                }

                return employeeClients;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
