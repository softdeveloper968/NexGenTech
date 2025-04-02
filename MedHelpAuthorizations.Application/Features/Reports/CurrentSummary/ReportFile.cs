namespace MedHelpAuthorizations.Application.Features.Reports.CurrentSummary
{
    public class ReportFile
    {
        public string Name { get; set; }
        public string File { get; set; }
        public ReportFile(string name, string file)
        {
            Name = name;
            File = file;
        }
    }
}
