using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Game
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public Scenario Scenario { get; set; }

        public Hero Master { get; set; }

        public List<Hero> Heroes { get; set; }
    }
}