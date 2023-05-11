using Microsoft.AspNetCore.SignalR;

namespace Communications.SignalR.Extensions;

public static class HubsContextExtensions
{
    public static async Task SendUpdateToGroup<THub, TObject>(this IHubContext<THub> hubContext, 
        string groupName, 
        string method,
        TObject message,
        CancellationToken cancellationToken = default)
        where THub : Hub
    {
        await hubContext.Clients
            .Group(groupName)
            .SendAsync(method, message, cancellationToken);
    }
}