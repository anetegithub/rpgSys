using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Scenario
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public string Title { get; set; }

        public string Recomendation { get; set; }

        public string Fable { get; set; }

        public List<Location> Locations { get; set; }

        public List<Event> Events { get; set; }

        public List<Npc> Npcs { get; set; }

        public List<Reward> Rewards { get; set; }
    }
}