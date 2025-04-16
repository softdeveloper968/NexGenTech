using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;

namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetAllPaged
{
    public class GetAllPatientQuestionnairesQuery : IRequest<PaginatedResult<GetAllPagedPatientQuestionnairesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PatientId { get; set; }

        public GetAllPatientQuestionnairesQuery(int pageNumber, int pageSize, int patientId)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PatientId = patientId;
        }
    }

    public class GetAllPatientQuestionnairesQueryHandler : IRequestHandler<GetAllPatientQuestionnairesQuery, PaginatedResult<GetAllPagedPatientQuestionnairesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserClientRepository _userClientRepository;
        private readonly IUserService _userService;

        public GetAllPatientQuestionnairesQueryHandler(IUnitOfWork<int> unitOfWork, IUserClientRepository userClientRepository, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userClientRepository = userClientRepository;
            _userService = userService;
        }

        public async Task<PaginatedResult<GetAllPagedPatientQuestionnairesResponse>> Handle(GetAllPatientQuestionnairesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<PatientQuestionnaire, GetAllPagedPatientQuestionnairesResponse>> expression = e => new GetAllPagedPatientQuestionnairesResponse
            {
                Id = e.Id,
                Name = e.ClientQuestionnaire.Name,
                AuthNumber = e.Authorization.AuthNumber,
                PatientId = e.PatientId,
                PatientQuestionnaireAnswers = e.PatientQuestionnairesAnswers.ToList(),
                CreatedOn = e.CreatedOn,
                CreatedBy = _userService.GetNameAsync(e.CreatedBy).Result,
                AuthorizationId = e.AuthorizationId,
                ClientQuestionnaireId = e.ClientQuestionnaireId,
            };
            var patientFilterSpec = new PatientQuestionnaireSpecification(request.PatientId);

            var data = await _unitOfWork.Repository<PatientQuestionnaire>().Entities
               .Specify(patientFilterSpec)
               .Select(expression)             
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }
    }
}