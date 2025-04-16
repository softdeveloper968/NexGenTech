using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class AddEmployeeClientUserReportFilterCommand : IRequest<Result<int>>
    {
        public int ClientUserReportId { get; set; }//AA-204//EN-61
        public List<EmployeeClientReportFilterDTO> EmployeeClientUserReportFilters { get; set; }//AA-204//EN-61//DTo

    }
    public class AddEmployeeClientUserReportFilterCommandHandler : IRequestHandler<AddEmployeeClientUserReportFilterCommand, Result<int>>
    {
        private readonly ICurrentUserService _userService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEmployeeClientUserReportFilterCommandHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IClientReportFilterService _clientReportFilterService;

        public AddEmployeeClientUserReportFilterCommandHandler(ICurrentUserService userService, IUnitOfWork<int> unitOfWork, IStringLocalizer<AddEmployeeClientUserReportFilterCommandHandler> localizer, IMapper mapper, IMediator mediator, IClientReportFilterService clientReportFilterService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
            _clientReportFilterService = clientReportFilterService;
        }

        public async Task<Result<int>> Handle(AddEmployeeClientUserReportFilterCommand command, CancellationToken cancellationToken)
        {
            try
            {

                if (command.ClientUserReportId == 0)
                {
                    return await Result<int>.FailAsync(_localizer["ClientUserReportId is Required!"]);
                }

                if (command.EmployeeClientUserReportFilters is null)
                {
                    ///Every record wipe out for that EmployeeClientUserReportFilter  based on ClientUserReportFilterId
                    command.EmployeeClientUserReportFilters = new();
                }

                ClientUserReportFilter clientUserReport = await _unitOfWork.Repository<ClientUserReportFilter>().Entities
                            .Include(z => z.EmployeeClientUserReportFilters)
                                .ThenInclude(z => z.EmployeeClient)
                                    .ThenInclude(z => z.Employee)
                            .FirstOrDefaultAsync(z => z.Id == command.ClientUserReportId, cancellationToken);
                if (clientUserReport == null)
                {
                    return await Result<int>.FailAsync(_localizer[$"ClientUserReport not found! Id: {command.ClientUserReportId}"]);
                }
                
                List<EmployeeClientUserReportFilter> employeeClientUserFiltersToRemove = clientUserReport.EmployeeClientUserReportFilters?.Where(x => command.EmployeeClientUserReportFilters.Any(item => item?.EmployeeClientId == x.EmployeeClientId)).ToList() ?? new List<EmployeeClientUserReportFilter>();

                List<EmployeeClientUserReportFilter> employeeClientUserFiltersToAdd = command.EmployeeClientUserReportFilters?.Where(x => employeeClientUserFiltersToRemove.All(item => item?.EmployeeClientId == x.EmployeeClientId)).Select(z => _mapper.Map<EmployeeClientUserReportFilter>(z) ?? new EmployeeClientUserReportFilter()).ToList();


                _unitOfWork.Repository<EmployeeClientUserReportFilter>().RemoveRange(employeeClientUserFiltersToRemove);
                _unitOfWork.Repository<EmployeeClientUserReportFilter>().AddRange(employeeClientUserFiltersToAdd);

                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(employeeClientUserFiltersToAdd.FirstOrDefault().Id, _localizer[$"EmployeeClientUserReportFilter {string.Join(",", employeeClientUserFiltersToAdd.Select(c => c.Id).ToList())} saved successfully!"]);

            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
