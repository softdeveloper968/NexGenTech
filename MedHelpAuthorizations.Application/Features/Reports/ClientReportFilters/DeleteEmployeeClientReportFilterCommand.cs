using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class DeleteEmployeeClientReportFilterCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteEmployeeClientReportFilterCommandHandler : IRequestHandler<DeleteEmployeeClientReportFilterCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteEmployeeClientReportFilterCommandHandler> _localizer;

        public DeleteEmployeeClientReportFilterCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteEmployeeClientReportFilterCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteEmployeeClientReportFilterCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var clientReportFilter = await _unitOfWork.Repository<EmployeeClientUserReportFilter>().GetByIdAsync(command.Id).ConfigureAwait(false) ?? null;
                if (clientReportFilter is not null)
                {
                    await _unitOfWork.Repository<EmployeeClientUserReportFilter>().DeleteAsync(clientReportFilter);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(command.Id, _localizer["Shared Report Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Shared Report Not Found!."]);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}
