using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Collections;

namespace RuneFramework
{
    public class GenericListLetter<T> : ILetter<T>
    {
        public bool NeedRune()
        { return true; }

        public bool NeedRuneChanges()
        { return true; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        {
            if ((ObjectAtRunic as IDictionary<string, object>).ContainsKey(Property.Name))
            {
                if ((ObjectAtRunic as IDictionary<string, object>)[Property.Name].ToString() != "")
                {
                    RuneList RList = (RuneList)(ObjectAtRunic as IDictionary<string, object>)[Property.Name];
                    var ListOfItems = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Property.PropertyType.GetGenericArguments()[0]));

                    foreach (var Item in RList.List)
                    {
                        int Id = Int32.Parse(Item.ToString());
                        string IdName;

                        if (Property.PropertyType.GetGenericArguments()[0].GetProperty("Id") == null)
                            IdName = Property.PropertyType.Name + "Id";
                        else
                            IdName = "Id";


                        foreach (PropertyInfo RuneWord in Rune.GetType().GetProperties())
                        {
                            if (RuneWord.Name == Property.Name)//Property.PropertyType.GetGenericArguments()[0].Name)
                            {
                                var Value = RuneWord.GetValue(Rune, null)
                                    .GetType()
                                    .GetMethod("QueryUniq")
                                    .Invoke(RuneWord.GetValue(Rune, null),
                                    new object[] { new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(IdName, "==", Id) } } });


                                ListOfItems.Add(Value);
                            }
                        }
                    }

                    Property.SetValue(Object, ListOfItems);
                }
            }
        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {

        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            RuneList RList = new RuneList() { List = new List<object>(), TypeName = Property.PropertyType.GetGenericArguments()[0].Name };

            var List = (IList)Property.GetValue(Object, null);
            if (List != null)
                foreach (var Item in List)
                {
                    string IdName;
                    if (Property.PropertyType.GetGenericArguments()[0].GetProperty("Id") == null)
                        IdName = Property.PropertyType.Name + "Id";
                    else
                        IdName = "Id";

                    RList.List.Add(Item.GetType().GetProperty(IdName).GetValue(Item, null).ToString());
                }

            (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, RList);
        }

        public void NeedChanges(out bool Result, T ObjectA, T ObjectB, PropertyInfo Property)
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
                if (ObjectA.GetType().GetInterface("IList") != null)
                    FieldByFieldCompareCollection(out Result, Property, EnumA.Current, EnumA.Current);
                else
                    if (Property.PropertyType.GetInterface("IList") != null)
                        FieldByFieldCompare(out Result, Property.PropertyType.GetGenericArguments()[0], EnumA.Current, EnumB.Current);
                    else
                        FieldByFieldCompare(out Result, Property.PropertyType, EnumA.Current, EnumB.Current);
            }
        }

        private void FieldByFieldCompareCollection(out bool Result, PropertyInfo Property, object A, object B)
        {
            Result = false;
            foreach (var InnerProperty in Property.PropertyType.GetGenericArguments()[0].GetProperties())
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
                    if (InnerProperty.PropertyType.GetInterface("IList") == null)
                        FieldByFieldCompare(out Result, InnerProperty.PropertyType, AProperty, BProperty);
                    else
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);

                    //FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                }
            }
        }

        private void FieldByFieldCompare(out bool Result, Type Property, object A, object B)
        {
            Result = false;

            foreach (var InnerProperty in Property.GetProperties())
            {
                var AProperty = InnerProperty.GetValue(A, null);
                var BProperty = InnerProperty.GetValue(B, null);

                if (!InnerProperty.PropertyType.IsClass || InnerProperty.PropertyType == typeof(String) || InnerProperty.PropertyType == typeof(RuneString))
                {
                    if (InnerProperty.PropertyType == typeof(String))
                    {
                        AProperty = AProperty ?? "";
                        BProperty = BProperty ?? "";
                    }

                    if (AProperty.Equals(BProperty))
                        Result = false;
                    else
                        Result = true;
                }
                else
                {
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

                    if (InnerProperty.PropertyType.GetInterface("IList") == null)
                        FieldByFieldCompare(out Result, InnerProperty.PropertyType, AProperty, BProperty);
                    else
                        FieldByFieldCompareCollection(out Result, InnerProperty, AProperty, BProperty);
                    //NeedChangesCollection(out Result, AProperty, BProperty, InnerProperty);
                }
            }
        }

        public void Dispose()
        { }
    }
}
