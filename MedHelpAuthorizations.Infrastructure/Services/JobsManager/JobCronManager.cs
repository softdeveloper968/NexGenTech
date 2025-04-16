using System;

namespace MedHelpAuthorizations.Infrastructure.Services.JobsManager
{
    public class JobCronManager : IJobCronManager
    {
        public string EnvironmentType { get; private set; }

        // Constructor to initialize the environment type
        public JobCronManager(string environmentType)
        {
            EnvironmentType = environmentType ?? throw new ArgumentNullException(nameof(environmentType));
        }

        // Method to check if the environment is Production
        public bool IsProductionEnvironment => EnvironmentType.Equals("Prod", StringComparison.OrdinalIgnoreCase);

        // Method to get the appropriate cron expression based on the environment
        public string GetCronExpression(string defaultCronExpression)
        {
            // If in production, return the default cron expression
            // Otherwise, return a default cron expression for non-production environments
            return IsProductionEnvironment ? defaultCronExpression : "0 0 31 2 0"; // Non-production default
        }

        public string GetEnvironmentType()
        {
            if (EnvironmentType.Equals("Prod", StringComparison.OrdinalIgnoreCase))
                return "Production";

            if (EnvironmentType.Equals("Beta", StringComparison.OrdinalIgnoreCase))
                return "Beta";

            if (EnvironmentType.Equals("Local", StringComparison.OrdinalIgnoreCase))
                return "Local";

            return "Unknown";
        }
    }
}
