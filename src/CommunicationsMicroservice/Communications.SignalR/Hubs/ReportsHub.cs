namespace Communications.SignalR.Hubs;

public class ReportsHub : BaseCommunicationsHub
{
    public static string GroupName => "ReportsGroup";

    public async Task JoinGroupAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);
    }

    public async Task LeaveGroupAsync()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName);
    }
}