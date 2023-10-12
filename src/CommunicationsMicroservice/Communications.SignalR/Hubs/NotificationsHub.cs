namespace Communications.SignalR.Hubs;

public class NotificationsHub : BaseCommunicationsHub
{
    public static string GroupName => "NotificationsGroup";

    public virtual async Task JoinGroupAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);
    }

    public virtual async Task LeaveGroupAsync()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName);
    }
}