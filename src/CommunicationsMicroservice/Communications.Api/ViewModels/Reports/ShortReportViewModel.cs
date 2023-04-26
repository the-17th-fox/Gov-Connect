using Communications.Api.ViewModels.Classifications;
using Communications.Core.Misc;

namespace Communications.Api.ViewModels.Reports;

public class ShortReportViewModel
{
    public Guid Id { get; set; }
    public string Header { get; set; } = string.Empty;
    public List<ClassificationViewModel> Classifications { get; set; } = new();
    public DateTime UpdatedAt { get; set; }
    public ReportStatuses ReportStatus { get; set; } = ReportStatuses.Pending;
}