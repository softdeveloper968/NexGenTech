using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;

namespace MedHelpAuthorizations.Application.Features.Patients.Queries.GetById
{
    public class GetPatientByIdQuery : IRequest<Result<GetPatientByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Result<GetPatientByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPatientByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<Result<GetPatientByIdResponse>> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Repository<Patient>()
                .Entities
                .Include(x => x.Person)
                .ThenInclude(y => y.Address)
                .FirstOrDefaultAsync(x => x.Id == query.Id);
            
            return await Result<GetPatientByIdResponse>.SuccessAsync(_mapper.Map<GetPatientByIdResponse>(patient));
        }
    }
}