using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using RuneFramework;

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
            Hero h = new Hero();
            using(var db =new Runes.HeroInfoRune())
            {
                h = (Hero)db.Hero.QueryUniq("Id", "==", HeroId);
            }
            Clients.All.syncheroexit(GameId, h);
        }

        public void syncgamedelete(String GameId)
        {
            Clients.All.syncgamedelete(GameId);
        }
    }
}