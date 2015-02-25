using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Ability
    {
        [attributeCL]
        public int Id { get; set; }

        [attributeCL]
        public int HeroId { get; set; }

        [referenceCL("Hero/Common/AbilityInfo")]
        [outerCL("Id")]
        public AbilityInfo Info { get; set; }

        public int Value { get; set; }
    }

    public class AbilityInfo
    {
        [attributeCL]
        public int Id { get; set; }
        [absorbedCL]
        public string Name { get; set; }
    }
}