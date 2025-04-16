using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Queries.GetAll
{
    public class GetAllRpaInsurancesQuery : IRequest<Result<List<GetAllRpaInsurancesResponse>>>
    {
        public string SearchString { get; set; }

        public GetAllRpaInsurancesQuery(string searchString)
        {
            SearchString = searchString;
        }
    }

    public class GetAllRpaInsurancesQueryHandler : IRequestHandler<GetAllRpaInsurancesQuery, Result<List<GetAllRpaInsurancesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public GetAllRpaInsurancesQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetAllRpaInsurancesResponse>>> Handle(GetAllRpaInsurancesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<RpaInsurance, GetAllRpaInsurancesResponse>> expression = e => new GetAllRpaInsurancesResponse
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    RpaInsuranceGroupName = e.RpaInsuranceGroup.Name
                };

                var data = await _unitOfWork.Repository<RpaInsurance>().Entities.
                    Include(r => r.RpaInsuranceGroup)
                   .Specify(new GenericNameSearchSpecification<RpaInsurance>(request.SearchString))
                   .OrderBy(x => x.Name)
                   .Select(expression)
                   .ToListAsync();

                return await Result<List<GetAllRpaInsurancesResponse>>.SuccessAsync(data);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}