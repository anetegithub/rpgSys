using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using System;

namespace rpgSys
{
    [HubName("play")]
    public class PlayHub : Hub
    {
        /*
         
         * Template:
         * 
         * DoThings
         * 
         * UpdateClient
         * 
         * end
         
         */

        public void Send(string GameId, string HeroId, string Master, string System, string Text)
        {
            //Add message to base
            xmlBase.Chat.Set(Convert.ToInt32(GameId), Convert.ToInt32(HeroId), Convert.ToBoolean(Master), Convert.ToBoolean(System), Text);

            //update clients
            Clients.All.update_chat();
        }

        public void Location(string GameId, string MasterId, string LocationId)
        {
            //change location
            xmlBase.Games.ChangeLocation(GameId, MasterId, LocationId);

            //update clients
            Clients.All.update_place();
        }

        public void Event(string GameId, string MasterId, string LocationId)
        {
            //change location
            xmlBase.Games.ChangeEvent(GameId, MasterId, LocationId);

            //update clients
            Clients.All.update_place();
        }

        public void Npc(string GameId, string MasterId, string LocationId)
        {
            //change location
            xmlBase.Games.ChangeNpc(GameId, MasterId, LocationId);

            //update clients
            Clients.All.update_place();
        }
    }
}