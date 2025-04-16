using AutoMapper;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit.Base;
using MedHelpAuthorizations.Application.Features.ReferringProviders.Commands.AddEdit.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ReferringProviders.Commands.AddEdit
{
    public class AddEditReferringProviderCommand : AddEditReferringProviderCommandBase, IRequest<Result<int>>
    {

    }

    public class AddEditReferringProviderCommandHandler : IRequestHandler<AddEditReferringProviderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditReferringProviderCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditReferringProviderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<AddEditReferringProviderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditReferringProviderCommand command, CancellationToken cancellationToken)
        {
            
            if (command.ProviderId == 0)
            {
                var provider = _mapper.Map<ReferringProvider>(command);
                provider.ClientId = _clientId;
                await _unitOfWork.Repository<ReferringProvider>().AddAsync(provider);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(provider.Id, _localizer["Referring Provider Saved"]);
            }
            else
            {
                var provider = await _unitOfWork.Repository<ReferringProvider>().GetByIdAsync(command.ProviderId);
                if (provider != null)
                {

                    provider.Npi = command.FaxNumber ?? provider.Npi;
                    provider.License = command.License ?? provider.License;                  
                    await _unitOfWork.Repository<ReferringProvider>().UpdateAsync(provider);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(provider.Id, _localizer["Provider Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Provider Not Found!"]);
                }
            }
        }
    }
}
