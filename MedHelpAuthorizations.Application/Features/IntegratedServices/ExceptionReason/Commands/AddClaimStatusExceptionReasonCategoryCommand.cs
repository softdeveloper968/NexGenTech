using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ExceptionReason.Queries;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ExceptionReason.Commands
{
    public class AddClaimStatusExceptionReasonCategoryCommand : IRequest<Result<int>>
    {
        public int ClaimStatusExceptionReasonCategoryId { get; set; }
        public string ClaimStatusExceptionReasonText { get; set; }
    }

    public class AddClaimStatusExceptionReasonCategoryCommandHandler : IRequestHandler<AddClaimStatusExceptionReasonCategoryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddClaimStatusExceptionReasonCategoryCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddClaimStatusExceptionReasonCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddClaimStatusExceptionReasonCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddClaimStatusExceptionReasonCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var data = _mapper.Map<ClaimStatusExceptionReasonCategoryMap>(command);
                await _unitOfWork.Repository<ClaimStatusExceptionReasonCategoryMap>().AddAsync(data);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(data.Id, _localizer["Saved"]);
            }
            catch (Exception ex)
            {
                string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message}";
                return await Result<int>.FailAsync(errorMessage);
            }
        }
    }
}
