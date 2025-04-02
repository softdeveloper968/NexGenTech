using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.Base;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.CreateAddress;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.UpdateAddress;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.UpsertAddresses
{
    public class UpsertAddressCommand : AddressCommand
    {

    }

    public class UpsertAddressCommandHandler : IRequestHandler<UpsertAddressCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private IMediator _mediator;

        public UpsertAddressCommandHandler(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(UpsertAddressCommand addressRequest, CancellationToken cancellationToken)
        {
            if (addressRequest.AddressId == 0)
            {
                var createAddressCommand = _mapper.Map<CreateAddressCommand>(addressRequest);
                
                return await _mediator.Send(createAddressCommand, cancellationToken);
            }
            else
            {
                var updateAddressCommand = _mapper.Map<UpdateAddressCommand>(addressRequest);
                return await _mediator.Send(updateAddressCommand, cancellationToken);
            }          
        }
    }
}
