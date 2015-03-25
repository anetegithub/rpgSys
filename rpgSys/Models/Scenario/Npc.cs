using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Npc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string Specification { get; set; }

        public NpcStat NpcStats { get; set; }
    }
}