using Microsoft.AspNetCore.SignalR;

namespace Communications.SignalR.Hubs;

public abstract class BaseCommunicationsHub : Hub
{
    public const string NewMessageMethodName = "NewMessage";
}