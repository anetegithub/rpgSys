using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public class GameChatMessage
    {
        public int Id { get; set; }
        public int GameId { get; set; }

        public RuneString GameMessageType { get; set; }

        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Stamp { get; set; }
        public string Text { get; set; }
    }
}