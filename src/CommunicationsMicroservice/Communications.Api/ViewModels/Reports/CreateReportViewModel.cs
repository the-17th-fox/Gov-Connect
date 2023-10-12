namespace Communications.Api.ViewModels.Reports;

public class CreateReportViewModel
{
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<Guid> ClassificationsIds { get; set; } = new();
}