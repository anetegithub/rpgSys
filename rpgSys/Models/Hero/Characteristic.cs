using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Dynamic;

using ormCL.Attributes;

namespace rpgSys
{
    public class Characteristic
    {
        [attributeCL]
        public int Id { get; set; }

        [attributeCL]
        public int HeroId { get; set; }

        [referenceCL("Hero/Common/CharacteristicName")]
        [outerCL("Id")]
        public string Name { get; set; }

        [referenceCL("Hero/Common/CharacteristicDIX")]
        [outerCL("Id")]
        public string DIX { get; set; }

        public int Value { get; set; }
    }
}