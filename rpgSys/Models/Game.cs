using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Game
    {
        public int Id { get; set; }
        public int ScriptId { get; set; }
        public int LocationId { get; set; }
        public int EventId { get; set; }
        public int NpcId { get; set; }
        public int Master { get; set; }
        public Int32[] Heroes { get; set; }
        public int Chat { get; set; }
    }
}