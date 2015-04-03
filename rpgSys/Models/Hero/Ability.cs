using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public sealed class Ability
    {
        public Int32 Id { get; set; }

        public RuneString AbilityName { get; set; }

        public Int32 Value { get; set; }

        public static Ability operator +(Ability A,Ability B)
        {
            if (A.AbilityName.Value == B.AbilityName.Value)
                A.Value += B.Value;
            return A;
        }

        public static Ability operator -(Ability A, Ability B)
        {
            if (A.AbilityName.Value == B.AbilityName.Value)
                A.Value -= B.Value;
            return A;
        }
    }
}