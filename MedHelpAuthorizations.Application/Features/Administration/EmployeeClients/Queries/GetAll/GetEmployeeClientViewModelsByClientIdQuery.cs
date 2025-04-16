using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications;
using Microsoft.AspNetCore.Identity;
using MedHelpAuthorizations.Application.Models.Identity;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Queries.GetAll
{
    public class GetEmployeeClientViewModelsByClientIdQuery : IRequest<Result<List<EmployeeClientViewModel>>>
    {
        public int ClientId { get; set; }

        public GetEmployeeClientViewModelsByClientIdQuery(int clientId)
        {
            ClientId = clientId;
        }
    }

    public class GetEmployeeClientViewModelsByClientIdQueryHandler : IRequestHandler<GetEmployeeClientViewModelsByClientIdQuery, Result<List<EmployeeClientViewModel>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetEmployeeClientViewModelsByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<Result<List<EmployeeClientViewModel>>> Handle(GetEmployeeClientViewModelsByClientIdQuery request, CancellationToken cancellationToken)
        {
            if(request.ClientId == 0) 
            {
                request.ClientId = _currentUserService.ClientId;
            }

            var employeeClients = await _unitOfWork.Repository<EmployeeClient>().Entities           
                .Include(x => x.Employee)
                .Include("EmployeeClientInsurances.ClientInsurance")
                .Include("EmployeeClientLocations.ClientLocation") 
                .Include("AssignedClientEmployeeRoles.EmployeeRole")
                .Include("EmployeeClientAlphaSplits")
                .Specify(new GenericByClientIdSpecification<EmployeeClient>(request.ClientId))
                .Select(x => _mapper.Map<EmployeeClientViewModel>(x))
                .ToListAsync();

            foreach (var employeeClient in employeeClients)
            {
                var user = await _userManager.FindByIdAsync(employeeClient.UserId);
                if (user != null)
                {
                    employeeClient.EmployeeName = user.LastName + "," + user.FirstName;
                }
            }

            return await Result<List<EmployeeClientViewModel>>.SuccessAsync(employeeClients);     
        }
    }
}
