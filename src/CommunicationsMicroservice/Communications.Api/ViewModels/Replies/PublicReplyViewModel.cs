namespace Communications.Api.ViewModels.Replies;

public class PublicReplyViewModel : ShortReplyViewModel
{
    public Guid AuthorityId { get; set; }
    public Guid ReportId { get; set; }
    public string Body { get; set; } = string.Empty;
}