using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Scenario
    {
        [attributeCL]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Recomendation { get; set; }

        public string Fable { get; set; }

        [referenceCL("/Scenario/Location")]
        [outerCL("Id")]
        public List<Location> Locations { get; set; }

        [referenceCL("/Scenario/Event")]
        [outerCL("Id")]
        public List<Event> Events { get; set; }

        [referenceCL("/Npc/Npc")]
        [outerCL("Id")]
        public List<Npc> Npcs { get; set; }

        [referenceCL("/Reward/Reward")]
        [outerCL("Id")]
        public List<Reward> Rewards { get; set; }
    }
}