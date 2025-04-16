using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientLocationRepositoryAsync : RepositoryAsync<ClientLocation, int>, IClientLocationRepository
    {

        private readonly ApplicationContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public ClientLocationRepositoryAsync(ApplicationContext dbContext, ICurrentUserService currentUserService) : base(dbContext)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        
        public ClientLocationRepositoryAsync(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //private readonly IRepositoryAsync<ClientLocation, int> _repository;
        //private readonly ApplicationContext _dbContext;
        //private readonly IMapper _mapper;
        
        //public ClientLocationRepositoryAsync(ApplicationContext dbContext, IRepositoryAsync<ClientLocation, int> repository, IMapper mapper, ICurrentUserService currentUserService) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //    _repository = repository;
        //    _mapper = mapper;
        //    _currentUserService = currentUserService;
        //}

        public IQueryable<ClientLocation> ClientLocations => _dbContext.ClientLocations;

        public async Task<ClientLocation> FindByNameAsync(string name)
        {
            var normalizedName = name?.ToUpper().Trim() ?? string.Empty;
            return await ClientLocations
               .Include(x => x.Address)
               .ThenInclude(y => y.State)
               .Specify(new GenericByClientIdSpecification<ClientLocation>(_clientId))
               .Where(cl => cl.Name.ToUpper().Trim() == normalizedName)
               .FirstOrDefaultAsync();
        }
        
        public async Task<ClientLocation> FindByEligibilityLocationIdAsync(int EligibilityLocationId)
        {
            return await ClientLocations
                         .FirstOrDefaultAsync(cloc => cloc.EligibilityLocationId == EligibilityLocationId);
        }
    }
}
