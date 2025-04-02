using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientDocumentRepository : RepositoryAsync<ClientDocument, int>, IClientDocumentRepository
    {
        private readonly ApplicationContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public ClientDocumentRepository(ApplicationContext dbContext, ICurrentUserService currentUserService) : base(dbContext)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public IQueryable<ClientDocument> ClientDocuments => _dbContext.ClientDocuments;

        public async Task<ClientDocument> GetByFileNameAndClientIdAsync(string fileName)
        {
            return await ClientDocuments
                .Include(x => x.Client)
                .Where(x => x.FileName.Equals(fileName) && x.ClientId == _clientId)
                .FirstOrDefaultAsync();
        }
    }

}
