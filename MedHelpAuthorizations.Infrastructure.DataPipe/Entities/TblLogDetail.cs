using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Models;

public partial class TblLogDetail
{
    public int LogId { get; set; }

    public string FileName { get; set; } = null!;

    public string PipeLineName { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string Status { get; set; } = null!;

    public string? ErrorMessage { get; set; }

    public DateTime? CreatedOn { get; set; }
}
