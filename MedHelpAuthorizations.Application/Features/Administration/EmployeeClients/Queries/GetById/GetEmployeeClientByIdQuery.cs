using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MedHelpAuthorizations.Application.Models.Identity;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Queries.GetById
{
    public class GetEmployeeClientByIdQuery : IRequest<Result<EmployeeClientDto>>
    {
        public int Id { get; set; }
    }

    public class GetEmployeeClientByIdQueryHandler : IRequestHandler<GetEmployeeClientByIdQuery, Result<EmployeeClientDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetEmployeeClientByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<EmployeeClientDto>> Handle(GetEmployeeClientByIdQuery query, CancellationToken cancellationToken)
        {
            var employeeClient = await _unitOfWork.Repository<EmployeeClient>().Entities
                .Include(x => x.Employee)
                .Include("EmployeeClientInsurances.ClientInsurance")
                .Include("EmployeeClientLocations.ClientLocation")
                .Include("AssignedClientEmployeeRoles.EmployeeRole")
                .Include("EmployeeClientAlphaSplits")
                //.Select(x => _mapper.Map<EmployeeClientDto>(x))
                .FirstOrDefaultAsync(ec => ec.Id == query.Id);

                var mappedEmployeeClient = _mapper.Map<EmployeeClientDto>(employeeClient);
                var user = await _userManager.FindByIdAsync(employeeClient.Employee.UserId); //AA-230
                if (user != null)
                {
                    mappedEmployeeClient.Employee.User = user;
                }
                return await Result<EmployeeClientDto>.SuccessAsync(mappedEmployeeClient);
        }
    }
}

