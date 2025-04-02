using System;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class InputDocumentRepository : RepositoryAsync<InputDocument, int>, IInputDocumentRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public InputDocumentRepository(ApplicationContext dbContext, ICurrentUserService currentUserService) : base(dbContext)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        public IQueryable<InputDocument> InputDocuments => _dbContext.InputDocuments;

        public async Task<InputDocument> GetByFileNameAndClientIdAsync(string fileName)
        {
            return await InputDocuments.Include(x=>x.ClientInsurance).Where(x => x.FileName.Equals(fileName) && x.ClientInsurance.ClientId == _clientId).FirstOrDefaultAsync();
        }
    }
}
