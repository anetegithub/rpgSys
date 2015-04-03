using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    internal sealed class HeroStuffRune : HeroRune
    {
        public RuneWord<Stuff> Items { get; set; }
    }
}