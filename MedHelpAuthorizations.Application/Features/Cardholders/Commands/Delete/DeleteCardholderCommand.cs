using MedHelpAuthorizations.Application.Features.Cardholders.Commands.Delete;
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

namespace MedHelpAuthorizations.Application.Features.Cardholders.Commands.Delete
{
    public class DeleteCardholderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteCardholderCommandHandler : IRequestHandler<DeleteCardholderCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCardholderCommandHandler> _localizer;

        public DeleteCardholderCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCardholderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCardholderCommand command, CancellationToken cancellationToken)
        {
            var cardholder = await _unitOfWork.Repository<Cardholder>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<Cardholder>().DeleteAsync(cardholder);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(cardholder.Id, _localizer["Cardholder Deleted"]);
        }
    }
}
