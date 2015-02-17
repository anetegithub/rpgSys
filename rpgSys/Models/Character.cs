using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Stat
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Value { get; set; }
        public string Bonus { get;set; }
    }


    public class Character
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }
        public string God { get; set; }
        public string Height { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public double Weight { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }
        public string Skin { get; set; }
    }
}