using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Dynamic;

namespace rpgSys
{
    public class Characteristics
    {
        public int Id { get; set; }
        public int HeroId { get; set; }
        public List<Characstic> Characteristic { get; set; }
    }

    public class Characstic
    {
        public string DIX { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}