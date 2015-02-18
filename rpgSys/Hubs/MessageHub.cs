using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace rpgSys
{
    [HubName("msg")]
    public class MessageHub : Hub
    {

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            xmlBase.Chat.add = message;
            //Clients.Caller.additem(name + ":" + message);
            Clients.All.update();

        }

        

        public void UpdateCl()
        {
            Clients.All.update();
        }
    }
}