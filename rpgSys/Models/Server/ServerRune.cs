using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public class ServerRune : Rune
    {
        public RuneWord<Server> Servers { get; set; }
        public RuneWord<GeneralChatMessage> GeneralChat { get; set; }
    }
}