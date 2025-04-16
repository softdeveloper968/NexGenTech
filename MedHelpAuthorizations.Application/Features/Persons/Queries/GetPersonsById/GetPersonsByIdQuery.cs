using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MediatR;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Exceptions;

namespace MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsById
{
    public class GetPersonByIdQuery : IRequest<Result<PersonDto>>
    {
        public int Id { get; set; }
        public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, Result<PersonDto>>
        {
            private readonly IPersonRepository _personRepository;
            private readonly IMapper _mapper;

            public GetPersonByIdQueryHandler(IPersonRepository personRepository, IMapper mapper)
            {
                _personRepository = personRepository;
                _mapper = mapper;
            }
            public async Task<Result<PersonDto>> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
            {
                //Probably have to override GetByIdAsync and include Address and State
                var person = await _personRepository.GetByIdAsync(query.Id);
                if (person == null)
                    throw new ApiException($"Person Not Found.");

                var personViewModel = _mapper.Map<PersonDto>(person);

                return await Result<PersonDto>.SuccessAsync(personViewModel); 
            }
        }
    }
}
