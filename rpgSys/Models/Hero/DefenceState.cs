using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public sealed class DefenceState
    {
        public Int32 Id { get; set; }

        public Int32 Defence { get; set; }
        public Int32 NaturalDefence { get; set; }
        public Int32 ArmorDefence { get; set; }
        public Int32 MagicDefence { get; set; }
        public Int32 DefenceClass { get; set; }
        public Int32 DefenceBonus { get; set; }

        public static DefenceState operator +(DefenceState A, DefenceState B)
        {
            A.Defence += B.Defence;
            A.NaturalDefence += B.NaturalDefence;
            A.ArmorDefence += B.ArmorDefence;
            A.MagicDefence += B.MagicDefence;
            A.DefenceClass += B.DefenceClass;
            A.DefenceBonus += B.DefenceBonus;

            return A;
        }

        public static DefenceState operator -(DefenceState A, DefenceState B)
        {
            A.Defence -= B.Defence;
            A.NaturalDefence -= B.NaturalDefence;
            A.ArmorDefence -= B.ArmorDefence;
            A.MagicDefence -= B.MagicDefence;
            A.DefenceClass -= B.DefenceClass;
            A.DefenceBonus -= B.DefenceBonus;

            return A;
        }
    }
}