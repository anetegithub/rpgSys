using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InnerName { get; set; }
        public string Info { get; set; }
        public double Version { get; set; }
        public string Dllinfo { get; set; }
    }
}