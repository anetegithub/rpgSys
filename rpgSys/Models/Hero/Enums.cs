using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Enums
    {
        [attributeCL]
        public int Id { get; set; }
        [absorbedCL]
        public string Value { get; set; }
    }
}