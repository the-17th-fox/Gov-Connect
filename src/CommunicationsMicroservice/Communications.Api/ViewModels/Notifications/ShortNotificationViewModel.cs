using Communications.Api.ViewModels.Classifications;

namespace Communications.Api.ViewModels.Notifications;

public class ShortNotificationViewModel
{
    public Guid Id { get; set; }
    public List<ClassificationViewModel> Classifications { get; set; } = new();
    public string Organization { get; set; } = string.Empty;
    public string Header { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}