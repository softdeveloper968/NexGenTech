using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Employees;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetEmployeeSearchQuery;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeService(IMapper mapper, IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }
        public async Task<List<EmployeeDto>> GetEmployeeSearch(GetEmployeeSearchQuery employeeSearchOptions)
        {
            var searchOption = await _employeeRepository.GetEmployeeSearch(employeeSearchOptions);
            var employeeMapping = _mapper.Map<List<EmployeeDto>>(searchOption);
            foreach (var employee in employeeMapping)
            {
                var user = await _userManager.FindByIdAsync(employee.UserId); //AA-206
                if (user != null)
                {
                    employee.User = user;
                }
            }
            if (!string.IsNullOrEmpty(employeeSearchOptions.FirstName))
            {
                employeeMapping = employeeMapping.Where(e => e.User.FirstName.Contains(employeeSearchOptions.FirstName)).ToList();
            }

            if (!string.IsNullOrEmpty(employeeSearchOptions.LastName))
            {
                employeeMapping = employeeMapping.Where(e => e.User.LastName.Contains(employeeSearchOptions.LastName)).ToList();
            }

            if (!string.IsNullOrEmpty(employeeSearchOptions.Email))
            {
                employeeMapping = employeeMapping.Where(e => e.User.Email.Contains(employeeSearchOptions.Email)).ToList();
            }

            if (!string.IsNullOrEmpty(employeeSearchOptions.MobilePhoneNumber.ToString()))
            {
                employeeMapping = employeeMapping.Where(e => long.Parse(e.User.PhoneNumber) == employeeSearchOptions.MobilePhoneNumber).ToList();
            }
            return employeeMapping;
        }
    }
}
