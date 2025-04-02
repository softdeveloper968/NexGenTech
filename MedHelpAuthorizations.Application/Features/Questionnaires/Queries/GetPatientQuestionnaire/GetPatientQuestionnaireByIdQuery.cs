using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetPatientQuestionnaire
{
    public class GetPatientQuestionnaireByIdQuery : IRequest<Result<GetPatientQuestionnaireByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetPatientQuestionnaireByIdHandler : IRequestHandler<GetPatientQuestionnaireByIdQuery, Result<GetPatientQuestionnaireByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPatientQuestionnaireByIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<GetPatientQuestionnaireByIdResponse>> Handle(GetPatientQuestionnaireByIdQuery query, CancellationToken cancellationToken)
        {
            var questionnaire = await _unitOfWork.Repository<PatientQuestionnaire>().GetByIdAsync(query.Id);
            var data = _mapper.Map<GetPatientQuestionnaireByIdResponse>(questionnaire);
            return await Result<GetPatientQuestionnaireByIdResponse>.SuccessAsync(data);
        }
    }
}
