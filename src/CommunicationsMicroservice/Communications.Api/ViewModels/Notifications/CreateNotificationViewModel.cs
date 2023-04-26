﻿namespace Communications.Api.ViewModels.Notifications;

public class CreateNotificationViewModel
{
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<Guid> ClassificationsIds { get; set; } = new();
}