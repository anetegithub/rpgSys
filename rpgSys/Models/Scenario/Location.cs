using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Location
    {
        [attributeCL]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Map { get; set; }
    }
}