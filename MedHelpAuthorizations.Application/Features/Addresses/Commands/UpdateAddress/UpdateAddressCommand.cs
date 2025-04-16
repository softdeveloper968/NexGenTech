using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.Base;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System;
using System.Net;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommand : AddressCommand
    {

    }
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Result<int>>
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateAddressCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public UpdateAddressCommandHandler(IAddressRepository addressRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<UpdateAddressCommandHandler> localizer)
        {
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpdateAddressCommand updateRequest, CancellationToken cancellationToken)
        {
            Address address;
            try
            {
                if (string.IsNullOrWhiteSpace(updateRequest.AddressStreetLine1))
                {
                    return await Result<int>.FailAsync(_localizer["AddressStreetLine1 is Required!"]);
                }

                //var address = await _addressRepository.GetByIdAsync(updateRequest.AddressId);
                address = await _unitOfWork.Repository<Address>().GetByIdAsync(updateRequest.AddressId).ConfigureAwait(false);

                if (address == null)
                {
                    throw new ApiException($"Address Not Found.");
                }
                else
                {
                    address = _mapper.Map<Address>(updateRequest);
                    //TODO: iisue hgert.. ID fdoes not mapt to addressID
                    await _unitOfWork.Repository<Address>().UpdateAsync(address);
                    await _unitOfWork.Commit(cancellationToken);
                    //await _addressRepository.UpdateAsync(address);

                    return await Result<int>.SuccessAsync(address.Id, _localizer["Address Updated"]);
                }
            }
            catch(Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[$"Error updating AddressId = {updateRequest.AddressId}" + ex.Message + Environment.NewLine + ex.InnerException?.Message]);
            }
        }
    }
}
