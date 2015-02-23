using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Dynamic;

using ormCL.Attributes;

namespace rpgSys
{
    public class Characteristics
    {
        [attributeCLAttribute]
        public int Id { get; set; }
        [attributeCLAttribute]
        public int HeroId { get; set; }
        [nameCL("Stats")]
        public List<Characstic> Characteristic { get; set; }
    }

    public class Characstic
    {
        [attributeCLAttribute]
        public string DIX { get; set; }
        [attributeCLAttribute]
        public string Name { get; set; }
        //[absorbedCL]
        public string Value { get; set; }
    }
}