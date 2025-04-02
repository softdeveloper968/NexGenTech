using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.InsuranceCards.Commands.Delete
{
    public class DeleteInsuranceCardCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteInsuranceCardCommandHandler : IRequestHandler<DeleteInsuranceCardCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteInsuranceCardCommandHandler> _localizer;

        public DeleteInsuranceCardCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteInsuranceCardCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteInsuranceCardCommand command, CancellationToken cancellationToken)
        {
            var insuranceCard = await _unitOfWork.Repository<InsuranceCard>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<InsuranceCard>().DeleteAsync(insuranceCard);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(insuranceCard.Id, _localizer["Insurance Card Deleted"]);
        }
    }
}
