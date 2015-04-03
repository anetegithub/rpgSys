using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public sealed class Characteristic
    {
        public Int32 Id { get; set; }

        public RuneString CharacteristicName { get; set; }
        public RuneString DIX { get; set; }

        public Int32 Value { get; set; }

        public static Characteristic operator +(Characteristic A, Characteristic B)
        {
            if (A.DIX.Value == B.DIX.Value)
                A.Value += B.Value;
            return A;
        }

        public static Characteristic operator -(Characteristic A, Characteristic B)
        {
            if (A.DIX.Value == B.DIX.Value)
                A.Value -= B.Value;
            return A;
        }
    }
}