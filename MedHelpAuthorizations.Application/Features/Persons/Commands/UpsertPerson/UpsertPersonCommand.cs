using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpdatePerson;
using MedHelpAuthorizations.Application.Features.Persons.Commands.CreatePerson;

namespace MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson
{
    public class UpsertPersonCommand : PersonCommand
    {

    }

    public class UpsertPersonCommandHandler : PersonCommandHandler<UpsertPersonCommand>
    {
        private readonly IMapper _mapper;
        private IMediator _mediator;

        public UpsertPersonCommandHandler(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public override async Task<Result<int>> Handle(UpsertPersonCommand personRequest, CancellationToken cancellationToken)
        {
            if (personRequest.PersonId != 0)
            {
                UpdatePersonCommand updateCommand = _mapper.Map<UpdatePersonCommand>(personRequest);
                return await _mediator.Send(updateCommand, cancellationToken);
            }
            else
            {
                CreatePersonCommand createCommand = _mapper.Map<CreatePersonCommand>(personRequest);
                return (await _mediator.Send(createCommand));
            }
        }
    }
}
