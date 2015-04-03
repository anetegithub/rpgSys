using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public sealed class AttackState
    {
        public Int32 Id { get; set; }

        public Int32 Attack { get; set; }
        public Int32 FitAttack { get; set; }
        public Int32 MinimalDamage { get; set; }
        public Int32 MaximalDamage { get; set; }
        public Int32 WeaponMinimalDamage { get; set; }
        public Int32 WeaponMaximalDamage { get; set; }
        public Int32 CritChance { get; set; }
        public Int32 CritBonus { get; set; }

        public static AttackState operator +(AttackState A, AttackState B)
        {
            A.Attack += B.Attack;
            A.FitAttack += B.FitAttack;
            A.MinimalDamage += B.MinimalDamage;
            A.MaximalDamage += B.MaximalDamage;
            A.WeaponMinimalDamage += B.WeaponMinimalDamage;
            A.WeaponMaximalDamage += B.WeaponMaximalDamage;
            A.CritChance += B.CritChance;
            A.CritBonus += B.CritBonus;

            return A;
        }

        public static AttackState operator -(AttackState A, AttackState B)
        {
            A.Attack -= B.Attack;
            A.FitAttack -= B.FitAttack;
            A.MinimalDamage -= B.MinimalDamage;
            A.MaximalDamage -= B.MaximalDamage;
            A.WeaponMinimalDamage -= B.WeaponMinimalDamage;
            A.WeaponMaximalDamage -= B.WeaponMaximalDamage;
            A.CritChance -= B.CritChance;
            A.CritBonus -= B.CritBonus;

            return A;
        }
    }
}