using Communications.Api.ViewModels.Classifications;

namespace Communications.Api.ViewModels.Notifications;

public class NotificationViewModel : ShortNotificationViewModel
{
    public List<ClassificationViewModel> Classifications { get; set; } = new();
    public string Organization { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}