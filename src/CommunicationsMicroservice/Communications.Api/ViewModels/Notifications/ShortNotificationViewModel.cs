namespace Communications.Api.ViewModels.Notifications;

public class ShortNotificationViewModel
{
    public Guid Id { get; set; }
    public string Header { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}