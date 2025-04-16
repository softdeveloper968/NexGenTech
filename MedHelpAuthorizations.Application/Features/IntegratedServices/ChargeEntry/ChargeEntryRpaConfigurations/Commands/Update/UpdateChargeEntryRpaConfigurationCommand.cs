using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Commands.Update
{
    using MedHelpAuthorizations.Domain.Entities.Enums;

    public class UpdateChargeEntryRpaConfigurationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public TransactionTypeEnum? TransactionTypeId { get; set; }
        public RpaTypeEnum? RpaTypeId { get; set; } // ICANOTES, et Cetera
        public int? AuthTypeId { get; set; }
        //public ServiceTypeEnum ServiceTypeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TargetUrl { get; set; }
        public string ReleaseKey { get; set; }
        public bool? FailureReported { get; set; } = false;
        public bool? IsDeleted { get; set; } = false;

        public class UpdateChargeEntryRpaConfigurationCommandHandler : IRequestHandler<UpdateChargeEntryRpaConfigurationCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IChargeEntryRpaConfigurationRepository _ChargeEntryRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public UpdateChargeEntryRpaConfigurationCommandHandler(IChargeEntryRpaConfigurationRepository ChargeEntryRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
            {

                _ChargeEntryRpaConfigurationRepository = ChargeEntryRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdateChargeEntryRpaConfigurationCommand command, CancellationToken cancellationToken)
            {
                var ChargeEntryRpaConfiguration = await _ChargeEntryRpaConfigurationRepository.GetByIdAsync(command.Id);

                if (ChargeEntryRpaConfiguration == null)
                {
                    return Result<int>.Fail($"ChargeEntryRpaConfiguration Not Found.");
                }
                else
                {
                    // Decided not to use the AutoMapper. 
                    // Decided to only alow update of certain columns
                    ChargeEntryRpaConfiguration.FailureReported = command.FailureReported ?? ChargeEntryRpaConfiguration.FailureReported;
                    ChargeEntryRpaConfiguration.TargetUrl = command.TargetUrl ?? ChargeEntryRpaConfiguration.TargetUrl;
                    ChargeEntryRpaConfiguration.Username = command.Username ?? ChargeEntryRpaConfiguration.Username;
                    ChargeEntryRpaConfiguration.Password = command.Password ?? ChargeEntryRpaConfiguration.Password;
                    ChargeEntryRpaConfiguration.IsDeleted = command.IsDeleted ?? ChargeEntryRpaConfiguration.IsDeleted;
                    ChargeEntryRpaConfiguration.AuthTypeId = command.AuthTypeId ?? ChargeEntryRpaConfiguration.AuthTypeId;

                    await _ChargeEntryRpaConfigurationRepository.UpdateAsync(ChargeEntryRpaConfiguration);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(ChargeEntryRpaConfiguration.Id);
                }
            }
        }
    }
}
