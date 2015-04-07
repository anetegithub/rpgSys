using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public class GameCommunicationRune : Rune
    {
        public RuneWord<Game> Game { get; set; }
        public RuneWord<Hero> Heroes { get; set; }
        public RuneWord<Hero> Master { get; set; }
        public RuneWord<User> User { get; set; }
    }
}