using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Application.Extensions;
using Microsoft.AspNetCore.Identity;
using static MedHelpAuthorizations.Shared.Constants.Permission.Permissions;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Queries.GetAll
{
    public class GetAllPagedEmployeeClientsQuery : IRequest<PaginatedResult<EmployeeClientDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllPagedEmployeeClientsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllEmployeeClientsQueryHandler : IRequestHandler<GetAllPagedEmployeeClientsQuery, PaginatedResult<EmployeeClientDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllEmployeeClientsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            userManager = _userManager;
        }

        public async Task<PaginatedResult<EmployeeClientDto>> Handle(GetAllPagedEmployeeClientsQuery request, CancellationToken cancellationToken)
        {
            var employeeClients = await _unitOfWork.Repository<EmployeeClient>().Entities
                .Include(x => x.Employee)
                .Include("EmployeeClientInsurances.ClientInsurance")
                .Include("EmployeeClientLocations.ClientLocation")
                .Include("AssignedClientEmployeeRoles.EmployeeRole")
                .Include("EmployeeClientAlphaSplits")
                .Select(x => _mapper.Map<EmployeeClientDto>(x))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            foreach (var employeeClient in employeeClients.Data) //AA-206
            {
                var user = await _userManager.FindByIdAsync(employeeClient.Employee.UserId);
                if (user == null)
                {
                    employeeClient.Employee.User = user;
                }
                var manager = await _userManager.FindByIdAsync(employeeClient.Employee.EmployeeManager.UserId);
                if (manager != null)
                {
                    employeeClient.Employee.EmployeeManager.User = manager;
                }
            }

            return employeeClients;
        }
    }
}
