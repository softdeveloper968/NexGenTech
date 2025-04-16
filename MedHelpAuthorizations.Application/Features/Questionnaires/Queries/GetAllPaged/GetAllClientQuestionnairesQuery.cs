using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;


namespace MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetAllPaged
{
    public class GetAllClientQuestionnairesQuery : IRequest<PaginatedResult<GetAllPagedClientQuestionnairesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //public string SearchString { get; set; }

        public GetAllClientQuestionnairesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllClientQuestionnaireQueryHandler : IRequestHandler<GetAllClientQuestionnairesQuery, PaginatedResult<GetAllPagedClientQuestionnairesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;


        public GetAllClientQuestionnaireQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }


        public async Task<PaginatedResult<GetAllPagedClientQuestionnairesResponse>> Handle(GetAllClientQuestionnairesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientQuestionnaire, GetAllPagedClientQuestionnairesResponse>> expression = e => new GetAllPagedClientQuestionnairesResponse
            {
                Id = e.Id,
                Name = e.Name,
                //ClientQuestionnaireCategories = e.ClientQuestionnaireCategories.ToList(),
                Description = e.Description,
                ClientQuestionnaire = e,
            };
            var clientFilterSpec = new QuestionnaireByClientIdSpecification(_clientId);

            var data = await _unitOfWork.Repository<ClientQuestionnaire>().Entities
               .Specify(clientFilterSpec)
               .Select(expression)               
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }

    }
}