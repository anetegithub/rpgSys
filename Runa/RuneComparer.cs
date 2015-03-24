using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace RuneFramework
{
    /// <summary>
    /// Objects types : Primitive, String, RuneString, PrimitiveList, GenericList of below types
    /// OR
    /// Lists of below types
    /// </summary>
    public static class RuneComparer
    {
        public static bool IsEqual(object A, object B)
        {
            if (A.GetType() != B.GetType())
                throw new ArgumentException(A.GetType().Name + " - A type is not same as " + B.GetType().Name + " - B type");

            return false;
        }

        private static bool CompareTwoPrimitives(object A, object B)
        {
            if (A == null && B == null)
                return true;
            else if ((A == null && B != null) || (A != null && B == null))
                return false;
            else if (A.GetType().IsPrimitive)
                return A.Equals(B);
            else if (A.GetType() == typeof(string))
                return (A as string).Equals(B as string);
            else if (A.GetType() == typeof(RuneString))
                return (A as RuneString).Equals((B as RuneString));
            else
                throw new NotSupportedException("The type '" + A.GetType().Name + "' is not supported by RuneComparer");
        }
    }
}
