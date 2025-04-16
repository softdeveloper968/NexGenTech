using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetQuestionnaireByClientId
{
    public class GetQuestionnaireByClientIdQuery : IRequest<Result<List<GetQuestionnaireByClientIdResponse>>>
    {
        public int ClientId { get; set; }
        public int QuestionnaireId { get; set; }
    }

    public class GetQuestionnaireByClientIdIdHandler : IRequestHandler<GetQuestionnaireByClientIdQuery, Result<List<GetQuestionnaireByClientIdResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetQuestionnaireByClientIdIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<List<GetQuestionnaireByClientIdResponse>>> Handle(GetQuestionnaireByClientIdQuery query, CancellationToken cancellationToken)
        {
            var questionnaire = await _unitOfWork.Repository<ClientQuestionnaire>()
                .Entities
                .Specify(new QuestionnaireByClientIdSpecification(query.ClientId))
                .Select(x => _mapper.Map<GetQuestionnaireByClientIdResponse>(x))
                .ToListAsync();            
            return await Result<List<GetQuestionnaireByClientIdResponse>>.SuccessAsync(questionnaire);
        }
    }
}
