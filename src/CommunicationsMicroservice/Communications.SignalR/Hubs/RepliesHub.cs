namespace Communications.SignalR.Hubs;

public class RepliesHub : BaseCommunicationsHub
{
    public async Task JoinGroupAsync(string reportId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, reportId);
    }

    public async Task LeaveGroupAsync(string reportId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, reportId);
    }
}