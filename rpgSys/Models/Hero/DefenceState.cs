using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class DefenceState
    {
        [attributeCL]
        public int Id { get; set; }
        [attributeCL]
        public int HeroId { get; set; }
        public int Defence { get; set; }
        public int NaturalDefence { get; set; }
        public int ArmorDefence { get; set; }
        public int MagicDefence { get; set; }
        public int DefenceClass { get; set; }
        public int DefenceBonus { get; set; }
    }
}