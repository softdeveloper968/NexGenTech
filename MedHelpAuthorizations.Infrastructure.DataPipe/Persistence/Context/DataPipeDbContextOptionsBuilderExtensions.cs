using MedHelpAuthorizations.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context
{
    public static class DataPipeDbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseDataPipeDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            switch (dbProvider?.ToLowerInvariant())
            {
                case DbProviderKeys.SqlServer:
                    return builder.UseSqlServer(connectionString, e =>
                         e.MigrationsAssembly("MedHelpAuthorizations.Infrastructure.DataPipe"));

                default:
                    throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
            }
        }
    }
}
