using SignalR;
using SignalR.Hubs;

namespace rpgSys
{
    [HubName("msg")]
    public class MessageHub : Hub { }
}