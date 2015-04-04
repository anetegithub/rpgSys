using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using System;

namespace rpgSys
{
    [HubName("lobby")]
    public class LobbyHub : Hub
    {
        public void gamedelete(string GameId)
        {
            Clients.All.gamedelete(GameId);
        }

        public void gamestart(string GameId)
        {
            Clients.All.gamestart(GameId);
        }

        public void listupdate(string GameId)
        {
            Clients.All.listupdate(GameId);
        }

        public void syncheroexit(String GameId, String HeroId)
        {
            Clients.All.syncheroexit(GameId, HeroId);
        }

        public void syncgamedelete(String GameId)
        {
            Clients.All.syncgamedelete(GameId);
        }
    }
}