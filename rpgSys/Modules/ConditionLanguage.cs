using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Dynamic;

namespace rpgSys
{
    public static class ConditionLanguage
    {
        [Obsolete("Run it is an old and not safe method, better use Satisfy. If you need set different object and type use SatisfyCustom method")]
        public static bool Run(object Object, Type Class, string Field, string Operator, string Value)
        {
            Field = Convert.ChangeType(Object, Class).GetType().GetProperty(Field).GetValue(Convert.ChangeType(Object, Class)).ToString();
            return ConditionLanguage.Compare<String>(Operator, Field, Value);
        }

        public static bool Satisfy(object Object, string Field, string Operator, string Value)
        {
            Field = Convert.ChangeType(Object, Object.GetType()).GetType().GetProperty(Field).GetValue(Convert.ChangeType(Object, Object.GetType())).ToString();
            return ConditionLanguage.Compare<String>(Operator, Field, Value);
        }

        /// <summary>
        /// Still working about it
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="CustomType"></param>
        /// <param name="Field"></param>
        /// <param name="Operator"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool SatisfyCustom(object Object, Type CustomType, string Field, string Operator, string Value)
        {
            try
            {
                Field = Convert.ChangeType(Object, Object.GetType()).GetType().GetProperty(Field).GetValue(Convert.ChangeType(Object, Object.GetType())).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return true;
            }
            return ConditionLanguage.Compare<String>(Operator, Field, Value);
        }

        private static bool Compare<T>(string op, T x, T y) where T : IComparable
        {
            switch (op)
            {
                case "==": return x.CompareTo(y) == 0;
                case "!=": return x.CompareTo(y) != 0;
                case ">": return x.CompareTo(y) > 0;
                case ">=": return x.CompareTo(y) >= 0;
                case "<": return x.CompareTo(y) < 0;
                case "<=": return x.CompareTo(y) <= 0;
                case "@=": return x.CompareToIn(y) >= 0;
                case "@!": return x.CompareToIn(y) < 0;
                case "%": return x.CompareToLike(y) != 0;
                case "!%": return x.CompareToLike(y) != 0;
                default: return false;
            }
        }

        private static int CompareToIn(this IComparable c, object obj)
        {
            string x = Convert.ToString(c);
            string y = Convert.ToString(obj);
            return x.IndexOf(y);
        }

        private static int CompareToLike(this IComparable c, object obj)
        {
            string x = Convert.ToString(c);
            string y = Convert.ToString(obj);

            int together = 0, length = (x.Length > y.Length ? x.Length : y.Length);

            for (int i = 0; i < length; i++)
            {
                char xc = ' ', yc = ' ';
                try { xc = x[i]; }
                catch { }
                try { yc = y[i]; }
                catch { }

                if (xc == yc)
                    together++;
            }

            return together >= length / 2 ? 1 : 0;
        }
    }
}