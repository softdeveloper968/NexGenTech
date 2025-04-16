using MedHelpAuthorizations.Application.Interfaces.Repositories;
using AutoMapper;
using MedHelpAuthorizations.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.Base;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.Extensions.Localization;
using static MedHelpAuthorizations.Shared.Constants.Permission.Permissions;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.CreateAddress
{
    public partial class CreateAddressCommand : AddressCommand
    {       

    }

    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Result<int>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateAddressCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        
        public CreateAddressCommandHandler(IAddressRepository addressRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<CreateAddressCommandHandler> localizer)
        {
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(CreateAddressCommand addressRequest, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(addressRequest.AddressStreetLine1))
            {
                return await Result<int>.FailAsync(_localizer["AddressStreetLine1 is Required!"]);
            }
            
            var address = _mapper.Map<Address>(addressRequest);
            address.ClientId = _clientId;
            
            await _unitOfWork.Repository<Address>().AddAsync(address);
            await _unitOfWork.Commit(cancellationToken);

            //address = await _addressRepository.AddAsync(address);
            return await Result<int>.SuccessAsync(address.Id, _localizer["Address Created"]);
        }
    }
}
