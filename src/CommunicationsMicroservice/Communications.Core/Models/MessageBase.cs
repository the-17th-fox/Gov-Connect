namespace Communications.Core.Models;

public abstract class MessageBase
{
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}