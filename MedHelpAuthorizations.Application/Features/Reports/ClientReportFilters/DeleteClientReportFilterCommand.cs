using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class DeleteClientReportFilterCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientReportFilterCommandHandler : IRequestHandler<DeleteClientReportFilterCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientReportFilterCommandHandler> _localizer;
        private readonly IClientReportFilterService _clientReportFilterService;

        public DeleteClientReportFilterCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientReportFilterCommandHandler> localizer, IClientReportFilterService clientReportFilterService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _clientReportFilterService = clientReportFilterService;
        }

        public async Task<Result<int>> Handle(DeleteClientReportFilterCommand command, CancellationToken cancellationToken)
        {
            try
            {
                ///Soft Delete Client User Report Filter
                return await _clientReportFilterService.DeleteById(command.Id);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}
