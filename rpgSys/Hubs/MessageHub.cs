using SignalR;
using SignalR.Hubs;

namespace rpgSys
{
    [HubName("msg")]
    public class MessageHub : Hub {


        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.additem(name + ":" + message);
        }
    }
}