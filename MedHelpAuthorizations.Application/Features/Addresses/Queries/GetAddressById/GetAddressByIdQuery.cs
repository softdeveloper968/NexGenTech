using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Addresses.ViewModels;
using AutoMapper;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetById;

namespace MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdQuery : IRequest<Result<GetAddressesViewModel>>
    {
        public int Id { get; set; }

        public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Result<GetAddressesViewModel>>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IMapper _mapper;

            public GetAddressByIdQueryHandler(IAddressRepository addressRepository, IMapper mapper)
            {
                _addressRepository = addressRepository;
                _mapper = mapper;
            }
            public async Task<Result<GetAddressesViewModel>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
            {
                var address = await _addressRepository.GetByIdAsync(query.Id);
                if (address == null) throw new ApiException($"Address Not Found.");

                var addressesViewModel = _mapper.Map<GetAddressesViewModel>(address);

                return await Result<GetAddressesViewModel>.SuccessAsync(addressesViewModel);
            }
        }
    }
}
