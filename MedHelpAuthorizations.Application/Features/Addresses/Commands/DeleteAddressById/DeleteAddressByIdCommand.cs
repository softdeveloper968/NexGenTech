using System.Threading;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.DeleteAddressById
{
    public class DeleteAddressByIdCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public class DeleteAddressByIdCommandHandler : IRequestHandler<DeleteAddressByIdCommand, Result<int>>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IStringLocalizer<AddEditProviderCommandHandler> _localizer;

            public DeleteAddressByIdCommandHandler(IAddressRepository addressRepository, IStringLocalizer<AddEditProviderCommandHandler> localizer)
            {
                _addressRepository = addressRepository;
                _localizer = localizer;
            }
            public async Task<Result<int>> Handle(DeleteAddressByIdCommand command, CancellationToken cancellationToken)
            {
                var address = await _addressRepository.GetByIdAsync(command.Id);
                if (address == null) 
                    throw new ApiException($"Address Not Found.");
                
                await _addressRepository.DeleteAsync(address);
                
                return await Result<int>.SuccessAsync(address.Id, _localizer["Address Deleted"]);
            }
        }
    }
}
