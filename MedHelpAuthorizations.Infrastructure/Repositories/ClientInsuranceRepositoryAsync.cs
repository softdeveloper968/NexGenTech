
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientInsuranceRepositoryAsync : RepositoryAsync<ClientInsurance, int>, IClientInsuranceRepository
    {
        private readonly IRepositoryAsync<ClientInsurance, int> _repository;
        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public ClientInsuranceRepositoryAsync(ApplicationContext dbContext, IRepositoryAsync<ClientInsurance, int> repository, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _repository = repository;
            _mapper = mapper;
        }

        public IQueryable<ClientInsurance> ClientInsurances => _dbContext.ClientInsurances; //EN-91

        public async Task<PaginatedResult<GetByCriteriaPagedInsurancesResponse>> GetByCriteria(GetByCriteriaPagedInsurancesQuery request)
        {
            //Completeby = _dbContext.Users.Where(z => z.Id == x.Completeby).Select(y => $"{y.LastName}, {y.FirstName}").FirstOrDefault(),
            var linq = _dbContext.ClientInsurances               
                .Specify(new ClientInsuranceFilterCriteriaSpecification(request))
                .Select(x => new GetByCriteriaPagedInsurancesResponse()
                {
                    Id = x.Id,
                    LookupName = x.LookupName,
                    Name = x.Name,
                    ClientId = x.ClientId,
                    PayerIdentifier = x.PayerIdentifier,
                    ExternalId = x.ExternalId,
                    PhoneNumber = x.PhoneNumber,
                    FaxNumber = x.FaxNumber
                });

            return await linq.ToPaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
