using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using System;

using ormCL;

namespace rpgSys
{
    [HubName("activity")]
    public class UserActivityHub : Hub
    {
        public void Send(string userName, string userAvatar, string msgText)
        {
            //baseCL b = new baseCL("Data");
            //int Id = b.Select(new requestCL() { Table = new tableCl("/Server/Chat") }).Cast<GeneralChatMessage>().ToList().Count + 1;
            //b.Insert<GeneralChatMessage>(new irequestCl()
            //{
            //    Table = new tableCl("/Server/Chat"),
            //    Object = new GeneralChatMessage() { Id = Id, UserName = userName, UserAvatar = userAvatar, Stamp = DateTime.Now, Text = msgText }
            //});
            //Clients.All.newmsg(userName, userAvatar, DateTime.Now.ToString(), msgText);
        }
    }
}