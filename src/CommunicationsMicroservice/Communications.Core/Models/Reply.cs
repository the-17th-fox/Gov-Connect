namespace Communications.Core.Models;

public class Reply : MessageBase
{
    public Guid Id { get; set; }
    public Guid AuthorityId { get; set; }
    public string Organization { get; set; } = string.Empty;

    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;

    public bool WasRead { get; set; } = false;
}