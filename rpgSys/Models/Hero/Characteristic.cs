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

        [referenceCL("Hero/Common/CharacteristicInfo")]
        [outerCL("Id")]
        public CharacteristicInfo Info { get; set; }

        public int Value { get; set; }
    }

    public class CharacteristicInfo
    {
        [attributeCL]
        public int Id { get; set; }

        public string Name { get; set; }

        public string DIX { get; set; }
    }
}