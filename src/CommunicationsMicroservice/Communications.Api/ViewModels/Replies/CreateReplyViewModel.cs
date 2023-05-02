namespace Communications.Api.ViewModels.Replies;

public class CreateReplyViewModel
{
    public Guid ReportId { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}