namespace Communications.Api.ViewModels.Replies;

public class ShortReplyViewModel
{
    public Guid Id { get; set; }
    public string Organization { get; set; } = string.Empty;

    public string Header { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}