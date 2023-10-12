namespace Communications.Api.ViewModels.Reports;

public class UpdateReportViewModel
{
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<Guid> ClassificationsIds { get; set; } = new();
}