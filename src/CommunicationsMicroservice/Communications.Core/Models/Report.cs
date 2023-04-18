using Communications.Core.Misc;

namespace Communications.Core.Models;

public class Report : MessageBase
{
    public Guid Id { get; set; }
    public Guid CivilianId { get; set; }

    public List<Classification> Classifications { get; set; } = new();

    public Guid? ReplyId { get; set; }
    public Reply? Reply { get; set; }

    public DateTime UpdatedAt { get; set; }
    public bool CanBeEdited => ReportStatus != ReportStatuses.Pending;
    
    public ReportStatuses ReportStatus { get; set; } = ReportStatuses.Pending;
    public bool IsForAuthoritiesOnly { get; set; }
}