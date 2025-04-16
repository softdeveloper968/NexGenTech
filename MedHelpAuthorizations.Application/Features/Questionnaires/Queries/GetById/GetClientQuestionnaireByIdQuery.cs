using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetById
{
    public class GetClientQuestionnaireByIdQuery : IRequest<Result<GetClientQuestionnaireByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetClientQuestionnaireByIdHandler : IRequestHandler<GetClientQuestionnaireByIdQuery, Result<GetClientQuestionnaireByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientQuestionnaireByIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<GetClientQuestionnaireByIdResponse>> Handle(GetClientQuestionnaireByIdQuery query, CancellationToken cancellationToken)
        {
            var questionnaire = await _unitOfWork.Repository<ClientQuestionnaire>().GetByIdAsync(query.Id);
            var data = _mapper.Map<GetClientQuestionnaireByIdResponse>(questionnaire);
            return await Result<GetClientQuestionnaireByIdResponse>.SuccessAsync(data);
        }
    }
}
