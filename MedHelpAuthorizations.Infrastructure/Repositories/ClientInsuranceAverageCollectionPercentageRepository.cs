using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientInsuranceAverageCollectionPercentageRepository : RepositoryAsync<ClientInsuranceAverageCollectionPercentage, int>, IClientInsuranceAverageCollectionPercentageRepository
    {
        private readonly ApplicationContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInsuranceAverageCollectionPercentageRepository"/> class.
        /// </summary>
        /// <param name="repository">The repository for client insurance average collection percentages.</param>
        public ClientInsuranceAverageCollectionPercentageRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ClientInsuranceAverageCollectionPercentage> ClientInsuranceAverageCollectionPercentages => _dbContext.ClientInsuranceAverageCollectionPercentages;

        public async Task<ClientInsuranceAverageCollectionPercentage> GetDataByQuarterAndClientInsurance(int quarter, string year, int clientInsuranceId)
        {
            try
            {
                return await ClientInsuranceAverageCollectionPercentages
                            .Specify(new SameYearQuarterClientInsuranceSpecification(quarter, year, clientInsuranceId.ToString()))
                            .FirstOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                // Handle exceptions appropriately or log them
                throw;
            }
        }
    }

}
