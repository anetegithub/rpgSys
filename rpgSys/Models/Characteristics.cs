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
        [attributeCL]
        public int Id { get; set; }
        [attributeCL]
        public int HeroId { get; set; }
        [nameCL("Stats")]
        public List<Characstic> Characteristic { get; set; }
        [referenceCL("/Hero/Character/AttackState")]
        [outerCL("HeroId")]
        public AttackState Attack { get; set; }
    }

    public class Characstic
    {
        [attributeCL]
        public string DIX { get; set; }
        [attributeCL]
        public string Name { get; set; }
        [absorbedCL]
        public string Value { get; set; }
    }

    public class AttackState
    {
        [attributeCL]
        public int HeroId { get; set; }
        public int Attack { get; set; }
        public int Fit { get; set; }
        public int MinDmg { get; set; }
        public int MaxDmg { get; set; }
        public int WeaponMinDmg { get; set; }
        public int WeaponMaxDmg { get; set; }
        public int CritChance { get; set; }
        public int CritBonus { get; set; }
    }
}