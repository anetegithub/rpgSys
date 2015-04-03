using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public class HeroInfoRune : Rune
    {
        public RuneWord<Hero> Hero { get; set; }
        public RuneWord<RuneString> Class { get; set; }
        public RuneWord<RuneString> Race { get; set; }
        public RuneWord<RuneString> Height { get; set; }
        public RuneWord<RuneString> Sex { get; set; }
    }
}