using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Npc
    {
        [attributeCL]
        public int Id { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string Specification { get; set; }
        [referenceCL("/Npc/NpcStat")]
        [outerCL("Id")]
        public NpcStat Stats { get; set; }
    }
}