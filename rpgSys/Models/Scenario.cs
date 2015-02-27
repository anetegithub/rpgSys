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

        [referenceCL("/Scenario/Npc")]
        [outerCL("Id")]
        public List<Npc> Npcs { get; set; }

        [referenceCL("/Scenario/Event")]
        [outerCL("Id")]
        public List<Event> Events { get; set; }

        [referenceCL("/Scenario/Reward")]
        [outerCL("Id")]
        public List<Item> Rewards { get; set; }
    }
    
    public class Location
    {
        [attributeCL]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Map { get; set; }        
    }

    public class Npc
    {
        [attributeCL]
        public int Id { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string Specification { get; set; }
        //public List<Stat> Stats { get; set; }
    }

    public class Event
    {
        [attributeCL]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}