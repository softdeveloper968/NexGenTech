namespace MedHelpAuthorizations.Infrastructure.Services.JobsManager
{
    public interface IJobCronManager
    {
        // Property to get the environment type
        string EnvironmentType { get; }

        // Method to check if the environment is Production
        bool IsProductionEnvironment { get; }

        // Method to get the appropriate cron expression based on the environment
        string GetCronExpression(string defaultCronExpression);

        string GetEnvironmentType();
    }

}
