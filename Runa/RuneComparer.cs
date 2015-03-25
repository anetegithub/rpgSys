using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Collections;

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
            bool R = true;
            if (A.GetType() != B.GetType())
                throw new ArgumentException(A.GetType().Name + " - A type is not same as " + B.GetType().Name + " - B type");
            else if (A.GetType().GetInterface("IList") != null)
            {
                bool temp = true;

                IEnumerator EnumA = (A as IList).GetEnumerator();
                IEnumerator EnumB = (B as IList).GetEnumerator();

                while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
                {
                    CompareTwoGenerics(A, B, ref R);
                }

                return temp;
            }
            else
                if (A.GetType().IsPrimitive || A.GetType() == typeof(String) || A.GetType() == typeof(RuneString))
                    CompareTwoPrimitives(A, B, ref R);
                else
                    CompareTwoGenerics(A, B, ref R);
            return R;
        }

        private static void CompareTwoGenerics(object A, object B, ref bool R)
        {
            if (IsNull(A, B) == -1)
            {
                if (A.GetType().GetInterface("IList") != null)
                {
                    IEnumerator EnumA = (A as IList).GetEnumerator();
                    IEnumerator EnumB = (B as IList).GetEnumerator();

                    while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
                    {
                        if (IsNull(EnumA.Current, EnumB.Current) == -1)
                        {
                            CompareTwoGenerics(EnumA.Current, EnumB.Current, ref R);
                        }
                        else if (IsNull(EnumA.Current, EnumB.Current) == 1)
                            R = false;
                    }
                }
                else
                    foreach (var Property in A.GetType().GetProperties())
                    {
                        var APropertyValue = Property.GetValue(A, null);
                        var BPropertyValue = Property.GetValue(B, null);

                        if (IsNull(APropertyValue, BPropertyValue) == -1)
                        {
                            if (APropertyValue.GetType().IsPrimitive || APropertyValue.GetType() == typeof(String) || APropertyValue.GetType() == typeof(RuneString))
                                CompareTwoPrimitives(APropertyValue, BPropertyValue, ref R);
                            else
                                CompareTwoGenerics(APropertyValue, BPropertyValue, ref R);
                        }
                        else if (IsNull(APropertyValue, BPropertyValue) == 1)
                            R = false;
                    }
            }
            else if (IsNull(A, B) == 1)
                R = false;
        }

        private static void CompareTwoPrimitives(object A, object B, ref bool R)
        {
            if (IsNull(A, B) == -1)
            {
                if (A.GetType().IsPrimitive)
                {
                    if (!A.Equals(B))
                        R = false;
                    return;
                }
                if (A.GetType() == typeof(string))
                {
                    if (!(A as string).Equals(B as string))
                        R = false;
                    return;
                }
                if (A.GetType() == typeof(RuneString))
                {
                    if (!(A as RuneString).Equals((B as RuneString)))
                        R = false;
                    return;
                }
                else
                    throw new NotSupportedException("The type '" + A.GetType().Name + "' is not supported by RuneComparer");
            }
            else if (IsNull(A, B) == 1)
                R = false;
        }

        private static int IsNull(object A, object B)
        {
            if (A == null && B == null)
                return 0;
            else if ((A == null && B != null) || (A != null && B == null))
                return 1;
            else
                return -1;
        }
    }
}