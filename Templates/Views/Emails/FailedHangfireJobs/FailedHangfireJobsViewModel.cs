namespace MedHelpAuthorizations.Template.Views.Emails.FailedHangfireJobs
{
    public class FailedHangfireJobsViewModel
    {
        public string Greeting { get; set; }
        public List<FailedHangfireJobs> FailedHangfireJobs { get; set; }
    }

    public class FailedHangfireJobs
    {
        public string JobId { get; set; }
        public string JobType { get; set; }
        public string FailedAt { get; set; }
        public string Exception { get; set; }
    }
}
