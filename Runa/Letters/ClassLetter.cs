using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Collections;

namespace RuneFramework
{
    public class ClassLetter<T> : ILetter<T>
    {
        public bool NeedRune()
        { return true; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                {
                    int Id = Int32.Parse(Field.Value.ToString());
                    string IdName;

                    if (Property.PropertyType.GetProperty("Id") == null)
                        IdName = Property.PropertyType.Name + "Id";
                    else
                        IdName = "Id";

                    foreach (PropertyInfo RuneWord in Rune.GetType().GetProperties())
                    {
                        if (RuneWord.Name == Property.Name)
                        {
                            var Value = RuneWord.GetValue(Rune, null)
                                .GetType()
                                .GetMethod("QueryUniq")
                                .Invoke(RuneWord.GetValue(Rune, null),
                                new object[] { new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(IdName, "==", Id) } } });

                            Property.SetValue(Object, Value);
                        }
                    }
                }
            }
        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        { }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);

            string Id;
            if (Property.PropertyType.GetProperty("Id") == null)
                Id = Property.PropertyType.Name + "Id";//typeof(T).Name + "Id";
            else
                Id = "Id";

            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value.GetType().GetProperty(Id).GetValue(Value, null).ToString());
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
        {
            var A = Property.GetValue(ObjectA, null) ?? Activator.CreateInstance(Property.PropertyType);
            var B = Property.GetValue(ObjectB, null) ?? Activator.CreateInstance(Property.PropertyType);

            Result = true;

            ABEquals(A, B, Property.PropertyType);
            //FieldByFieldCompare(ref Result, Property, A, B);
        }

        private bool ABEquals(object A, object B, Type TheyType)
        {
            // uses Reflection to check if a Type-specific `Equals` exists...
            var specificEquals = TheyType.GetMethod("Equals", new Type[] { TheyType });
            if (specificEquals != null &&
                specificEquals.ReturnType == typeof(bool))
            {
                return (bool)specificEquals.Invoke(A, new object[] { B });
            }
            return A.Equals(B);
        }

        private void FieldByFieldCompare(ref bool Result, PropertyInfo Property, object A, object B)
        {
            foreach (var InnerProperty in Property.PropertyType.GetProperties())
            {
                var AProperty = InnerProperty.GetValue(A, null);
                var BProperty = InnerProperty.GetValue(B, null);

                if (AProperty == null && BProperty == null)
                {
                    Result = false;
                    return;
                }

                if ((AProperty == null && BProperty != null) || (BProperty == null && AProperty != null))
                {
                    //Result = true;
                    return;
                }

                if (!InnerProperty.PropertyType.IsClass || InnerProperty.PropertyType == typeof(String) || InnerProperty.PropertyType == typeof(RuneString))
                {
                    if (InnerProperty.PropertyType == typeof(String))
                    {
                        AProperty = AProperty ?? "";
                        BProperty = BProperty ?? "";
                    }

                    if (AProperty.Equals(BProperty))
                        Result = false;
                    //else
                    //    Result = true;
                }
                else
                {

                    if (InnerProperty.PropertyType.GetInterface("IList") == null)
                        FieldByFieldCompare(ref Result, InnerProperty, AProperty, BProperty);
                    else
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                    //NeedChangesCollection(out Result, AProperty, BProperty, InnerProperty);
                }
            }
        }

        private void NeedChangesCollection(out bool Result, object ObjectA, object ObjectB, PropertyInfo Property)
        {
            Result = false;

            var ListA = (IList)Property.GetValue(ObjectA, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));
            var ListB = (IList)Property.GetValue(ObjectB, null) ?? (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));

            if (ListA.Count != ListB.Count)
            {
                Result = true;
                return;
            }

            IEnumerator EnumA = ListA.GetEnumerator();
            IEnumerator EnumB = ListB.GetEnumerator();

            while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
            {
                FieldByFieldCompareCollection(out Result, Property, EnumA.Current, EnumA.Current);
            }
        }

        private void FieldByFieldCompareCollection(out bool Result, PropertyInfo Property, object A, object B)
        {
            Result = false;
            foreach (var InnerProperty in Property.PropertyType.GetGenericArguments()[0].GetProperties())
            {
                IEnumerator EnumA = (A as IEnumerable).GetEnumerator();
                IEnumerator EnumB = (B as IEnumerable).GetEnumerator();

                while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
                {
                    var AProperty = InnerProperty.GetValue(EnumA.Current, null);
                    var BProperty = InnerProperty.GetValue(EnumB.Current, null);

                    if (AProperty == null && BProperty == null)
                    {
                        Result = false;
                        return;
                    }

                    if ((AProperty == null && BProperty != null) || (BProperty == null && AProperty != null))
                    {
                        Result = true;
                        return;
                    }

                    if (!InnerProperty.PropertyType.IsClass || InnerProperty.PropertyType == typeof(String) || InnerProperty.PropertyType == typeof(RuneString))
                    {
                        if (InnerProperty.PropertyType == typeof(String))
                        {
                            AProperty = AProperty ?? "";
                            BProperty = BProperty ?? "";
                        }

                        if (AProperty == null)

                            if (AProperty.Equals(BProperty))
                                Result = false;
                            else
                                Result = true;
                    }
                    else
                    {
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                    }
                }
            }
        }

        public void Dispose()
        { }
    }
}
