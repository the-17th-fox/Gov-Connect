using Microsoft.AspNetCore.SignalR;

namespace Communications.SignalR.Hubs;

public class RepliesHub : Hub
{
    public const string NewReplyMethodName = "NewReply";

    public async Task JoinReportGroupAsync(string reportId)
    {
        if (string.IsNullOrEmpty(reportId))
        {
            throw new ArgumentNullException(nameof(reportId));
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, reportId);
    }

    public async Task LeaveReportGroupAsync(string reportId)
    {
        if (string.IsNullOrEmpty(reportId))
        {
            throw new ArgumentNullException(nameof(reportId));
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, reportId);
    }
}