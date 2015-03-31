using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using System;

using RuneFramework;

namespace rpgSys
{
    [HubName("game")]
    public class GameHub : Hub
    {
        public void sendmsg(string gameId, string userName, string userAvatar, string userType, string userMsg)
        {
            Int32 GameId = 0, UserType = 0;

            if (!Int32.TryParse(gameId, out GameId))
                return;
            if (!Int32.TryParse(userType, out UserType))
                return;

            using (var db = new Runes.GameRune())
            {
                GameChatMessage gcm = new GameChatMessage();
                gcm.GameId = GameId;
                gcm.Name = userName;
                gcm.Avatar = userAvatar;
                gcm.Text = userMsg;
                gcm.Stamp = DateTime.Now.ToString();
                try { gcm.GameMessageType = db.GameMessageType.ReferenceUniq(new SimpleRuneSpell("Id", "==", UserType)); }
                catch (ArgumentException) { return; }
                db.Chat.Add(gcm);

                db.SaveRune();

                Clients.All.addgamemsg(gcm);
            }
        }
    }
}