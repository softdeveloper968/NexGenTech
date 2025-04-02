using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class AddEditClientReportFiltersCommand : IRequest<Result<int>>
    {
        public ReportsEnum ReportId { get; set; }
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string UserId { get; set; }//AA-193
        public bool SaveAsNewFilter { get; set; } = false;
        public bool HasDefaultFilter { get; set; }
        public bool RunSavedDefaultFilter { get; set; }
        public string FilterName { get; set; }
        public string FilterConfiguration { get; set; }
    }
    public class AddEditClientReportFiltersCommandHandler : IRequestHandler<AddEditClientReportFiltersCommand, Result<int>>
    {
        private readonly ICurrentUserService _userService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientReportFiltersCommandHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IClientReportFilterService _clientReportFilterService;

        public AddEditClientReportFiltersCommandHandler(ICurrentUserService userService, IUnitOfWork<int> unitOfWork, IStringLocalizer<AddEditClientReportFiltersCommandHandler> localizer, IMapper mapper, IMediator mediator, IClientReportFilterService clientReportFilterService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
            _clientReportFilterService = clientReportFilterService;
        }

        public async Task<Result<int>> Handle(AddEditClientReportFiltersCommand command, CancellationToken cancellationToken)
        {
            try
            {
                /// If clientId is not set in AddEditClientReportFilter command then set here...
                if (command.ClientId == 0)
                    command.ClientId = _userService.ClientId;

                if (string.IsNullOrEmpty(command.UserId))//AA-193
                    command.UserId = _userService.UserId;

                if (command.Id == 0)
                {
                    var reportFilter = _mapper.Map<ClientUserReportFilter>(command);
                    ///verify/update if AutoRun option enable then current report filter is set, 
                    ///update existing reportfilters to disabled from AutoRun as well as Default option.
                    if (command.RunSavedDefaultFilter || command.HasDefaultFilter)
                    {
                        _unitOfWork.Repository<ClientUserReportFilter>().ExecuteUpdate(p => p.ClientId == command.ClientId && p.ReportId == command.ReportId,
                            u =>
                            {
                                u.HasDefaultFilter = false;
                                u.RunSavedDefaultFilter = false;
                            });
                    }
                    await _unitOfWork.Repository<ClientUserReportFilter>().AddAsync(reportFilter);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(reportFilter.Id, _localizer["ClientReportFilters Saved"]);
                }
                else
                {
                    //var reportExist = await _unitOfWork.Repository<ClientReportFilter>().GetByIdAsync(command.Id);
                    var reportExist = await _clientReportFilterService.GetClientReportFilters(command.Id, command.ClientId, (ReportsEnum)command.ReportId);
                    if (reportExist != null)
                    {
                        if (command.RunSavedDefaultFilter || command.HasDefaultFilter)
                        {
                            _unitOfWork.Repository<ClientUserReportFilter>().ExecuteUpdate(p => p.ClientId == command.ClientId && p.ReportId == command.ReportId,
                                u =>
                                {
                                    u.HasDefaultFilter = false;
                                    u.RunSavedDefaultFilter = false;
                                });
                        }
                        reportExist = _mapper.Map<ClientUserReportFilter>(command);

                        await _unitOfWork.Repository<ClientUserReportFilter>().UpdateAsync(reportExist);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(reportExist.Id, _localizer["ReportFilter Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["ReportFilter Not Found!"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
