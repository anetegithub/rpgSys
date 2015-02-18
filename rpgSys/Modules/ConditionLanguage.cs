using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public static class ConditionLanguage
    {
        public static bool Run(object Object, Type Class, string Field, string Operator, string Value)
        {
            Field = Convert.ChangeType(Object, Class).GetType().GetProperty(Field).GetValue(Convert.ChangeType(Object, Class)).ToString();
            return ConditionLanguage.Compare<String>(Operator, Field, Value);
        }

        public static bool Compare<T>(string op, T x, T y) where T : IComparable
        {
            switch (op)
            {
                case "==": return x.CompareTo(y) == 0;
                case "!=": return x.CompareTo(y) != 0;
                case ">": return x.CompareTo(y) > 0;
                case ">=": return x.CompareTo(y) >= 0;
                case "<": return x.CompareTo(y) < 0;
                case "<=": return x.CompareTo(y) <= 0;
                default: return false;
            }
        }
    }
}