using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public sealed class Game
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public Scenario Scenario { get; set; }

        public Hero Master { get; set; }

        internal List<Hero> Heroes { get; set; }

        public List<Npc> Npcs { get; set; }

        public Event Event { get; set; }

        public Location Location { get; set; }
    }
}