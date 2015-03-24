using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using System;

using RuneFramework;

namespace rpgSys
{
    [HubName("generalchat")]
    public class GeneralChatHub : Hub 
    {
        public void Send(string userName, string userAvatar, string msgText)
        {
            using (var db = new Runes.ServerRune())
            {
                GeneralChatMessage gcm = new GeneralChatMessage() { UserAvatar = userAvatar, UserName = userName, Text = msgText };
                gcm.Stamp = DateTime.Now.ToString();
                db.GeneralChat.Add(gcm);
                db.SaveRune();                
            }
            Clients.All.newmsg(userName, userAvatar, DateTime.Now.ToString(), msgText);
        }
    }
}