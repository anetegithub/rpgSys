using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public class GameRune : Rune
    {
        public RuneWord<Game> Game { get; set; }
        public RuneWord<Scenario> Scenario { get; set; }
        public RuneWord<Hero> Heroes { get; set; }
        public RuneWord<Hero> Master { get; set; }
        public RuneWord<RuneString> Sex { get; set; }
    }
}