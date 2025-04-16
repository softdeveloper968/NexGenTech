using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Commands.Create
{
    /// <summary>
    /// The create charge entry rpa configuration command.
    /// </summary>
    public class CreateChargeEntryRpaConfigurationCommand : IRequest<Result<int>>
    {
        public int ClientId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }
        public RpaTypeEnum RpaTypeId { get; set; } // ICANOTES, et Cetera
        public int AuthTypeId { get; set; }
        //public ServiceTypeEnum ServiceTypeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TargetUrl { get; set; }
        public string LocationName { get; set; }
        public bool FailureReported { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// The create charge entry rpa configuration command handler.
    /// </summary>
    public class CreateChargeEntryRpaConfigurationCommandHandler : IRequestHandler<CreateChargeEntryRpaConfigurationCommand, Result<int>>
    {
        private readonly IChargeEntryRpaConfigurationRepository _chargeEntryRpaConfigurationRepository;

        /// <summary>
        /// The _mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Gets or sets the _unit of work.
        /// </summary>
        private IUnitOfWork<int> _unitOfWork { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateChargeEntryRpaConfigurationCommandHandler"/> class.
        /// </summary>
        /// <param name="chargeEntryRpaConfigurationRepository">
        /// The charge entry rpa configuration repository.
        /// </param>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public CreateChargeEntryRpaConfigurationCommandHandler(IChargeEntryRpaConfigurationRepository chargeEntryRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _chargeEntryRpaConfigurationRepository = chargeEntryRpaConfigurationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateChargeEntryRpaConfigurationCommand request, CancellationToken cancellationToken)
        {
            var chargeEntryRpaConfiguration = _mapper.Map<ChargeEntryRpaConfiguration>(request);
            await _chargeEntryRpaConfigurationRepository.InsertAsync(chargeEntryRpaConfiguration);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(chargeEntryRpaConfiguration.Id);
        }
    }
}
