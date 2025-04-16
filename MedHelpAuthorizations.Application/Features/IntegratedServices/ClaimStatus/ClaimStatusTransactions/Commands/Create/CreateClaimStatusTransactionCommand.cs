using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Base;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Create
{
    public class CreateClaimStatusTransactionCommand : BaseClaimStatusTransactionCommand
    {
    }
    public class CreateClaimStatusTransactionCommandHandler : IRequestHandler<CreateClaimStatusTransactionCommand, Result<int>>
    {
        private readonly IClaimStatusTransactionRepository _claimStatusTransactionRepository;
        private readonly IClaimStatusTransactionHistoryRepository _claimStatusTransactionHstoryRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateClaimStatusTransactionCommandHandler(IClaimStatusTransactionRepository claimStatusTransactionRepository, IClaimStatusTransactionHistoryRepository claimStatusTransactionHstoryRepository, IMapper mapper, IMediator mediator)
        {
            _claimStatusTransactionRepository = claimStatusTransactionRepository;
            _claimStatusTransactionHstoryRepository = claimStatusTransactionHstoryRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(CreateClaimStatusTransactionCommand command, CancellationToken cancellationToken)
        {
            UpsertClaimStatusTransactionCommand upsertTransactionCommand = _mapper.Map<UpsertClaimStatusTransactionCommand>(command);
            return await _mediator.Send(upsertTransactionCommand);
        }
    }
}
