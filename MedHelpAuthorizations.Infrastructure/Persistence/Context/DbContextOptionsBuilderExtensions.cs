using MedHelpAuthorizations.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Context
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            switch (dbProvider?.ToLowerInvariant())
            {

                case DbProviderKeys.SqlServer:
                    return builder.UseSqlServer(connectionString, e =>
                    {
                        e.MigrationsAssembly("MedHelpAuthorizations.Infrastructure"); 
                        e.EnableRetryOnFailure();
                    });

                default:
                    throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
            }
        }
    }
}
