using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class AttackState
    {
        [attributeCL]
        public int Id { get; set; }
        [attributeCL]
        public int HeroId { get; set; }
        public int Attack { get; set; }
        public int FitAttack { get; set; }
        public int MinimalDamage { get; set; }
        public int MaximalDamage { get; set; }
        public int WeaponMinimalDamage { get; set; }
        public int WeaponMaximalDamage { get; set; }
        public int CritChance { get; set; }
        public int CritBonus { get; set; }
    }
}