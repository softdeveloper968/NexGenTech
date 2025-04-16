using Microsoft.Extensions.Logging;
using MedHelpAuthorizations.Infrastructure.Shared.Persistence.Initialization;
using MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Initialization
{
    public partial class DfStagingDbSeeder
	{
        private readonly CustomSeederRunner _seederRunner;
        private readonly ILogger<DfStagingDbSeeder> _logger;
        private DfStagingDbContext _dbContext { get; set; }
      
        public DfStagingDbSeeder(CustomSeederRunner seederRunner, ILogger<DfStagingDbSeeder> logger)
        {
            _seederRunner = seederRunner;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync(DfStagingDbContext dbContext, CancellationToken cancellationToken)
        {
            try
            {
                //_dbContext = dbContext;
                //await _seederRunner.RunSeedersAsync(cancellationToken);

            }
            catch(Exception ex)
            {
                throw;
            }           
        }       
    }
}
