using System;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Addresses.ViewModels;
using System.Collections.Generic;
using AutoMapper;

namespace MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAddresses
{
    public class GetAddressByDeliveryPointQuery : IRequest<Result<GetAddressesViewModel>>
    {
        public int DeliveryPointBarCode { get; set; }
        public class GetAddressByDeliveryPointQueryHandler : IRequestHandler<GetAddressByDeliveryPointQuery, Result<GetAddressesViewModel>>
        {
            private readonly IAddressRepositoryAsync _addressRepository;
            public GetAddressByDeliveryPointQueryHandler(IAddressRepositoryAsync addressRepository)
            {
                _addressRepository = addressRepository;
            }
            public async Task<Result<GetAddressesViewModel>> Handle(GetAddressByDeliveryPointQuery query, CancellationToken cancellationToken)
            {
                var addressViewModel = await _addressRepository.FindByDeliveryPoint(query.DeliveryPointBarCode);
                if (addressViewModel == null) throw new ApiException($"Address Not Found.");

                return new Result<GetAddressesViewModel>(addressViewModel);
            }
        }
    }
}
