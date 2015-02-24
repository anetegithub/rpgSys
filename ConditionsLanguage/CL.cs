using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace ConditionsLanguage
{
    public static class CL
    {
        [Obsolete("Run is an old and don't safe method, better use Satisfy. If you need set different object and type use SatisfyCustom method")]
        public static bool Run(object Object, Type Class, string Field, string Operator, string Value)
        {
            Field = Convert.ChangeType(Object, Class).GetType().GetProperty(Field).GetValue(Convert.ChangeType(Object, Class)).ToString();
            return CL.Compare<String>(Operator, Field, Value);
        }

        public static bool Satisfy(object Object, string Field, string Operator, string Value)
        {
            Field = Convert.ChangeType(Object, Object.GetType()).GetType().GetProperty(Field).GetValue(Convert.ChangeType(Object, Object.GetType())).ToString();
            return CL.Compare<String>(Operator, Field, Value);
        }

        public static string[] Split(string Conditions)
        {
            return Conditions.Split(',');
        }

        public static bool Solve(object Object, string Condition)
        {
            string Operator = Condition.Split('.')[1];
            string Value = Condition.Split('.')[2];
            string Field = Convert.ChangeType(Object, Object.GetType()).GetType().GetProperty(Condition.Split('.')[0]).GetValue(Convert.ChangeType(Object, Object.GetType())).ToString();
            return CL.Compare<String>(Operator, Field, Value);
        }

        public static bool Solve(XElement Object, string Condition)
        {
            bool preResult = true;
            string Field=Condition.Split('.')[0];
            string Operator = Condition.Split('.')[1];
            string Value = Condition.Split('.')[2];
            foreach(XElement Element in Object.Elements())
            {
                if (Element.Name.LocalName==Field)
                {
                    preResult = Compare<String>(Operator, Element.Value, Value);
                }
            }
            return preResult;
        }

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
            return CL.Compare<String>(Operator, Field, Value);
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
                case "@": return x.CompareToIn(y) >= 0;
                case "!@": return x.CompareToIn(y) < 0;
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

        public static readonly string Rules =
@"
    Syntax:
        <Field.Operation.Value,Field.Operation.Value>
    Operations:
        1. Equal: ==
        2. NotEqual: !=
        3. More/Less: >/<
        4. More/Less or Equal: >=/<=
        5. Object like string CONTAINS string: @
        6. Object like string NOT CONTAINS string: !@
        7. Object like string LIKE another string MORE OR EQUAL 50% of SOURSE string: %
        8. Object like string NOT LIKE another string MORE OR EQUAL 50% of SOURSE string: !%

    SortingCL syntax:
        <Id:Desc,Name:Asce>
";
    }
}
