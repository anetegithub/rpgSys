using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rare { get; set; }
        public string Who { get; set; }
        public string Additional { get; set; }
        public Stat[] Characteristics { get; set; }
    }
}