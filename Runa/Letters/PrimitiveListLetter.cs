using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Collections;

namespace RuneFramework
{
    public class PrimitiveListLetter<T> : ILetter<T>
    {
        public bool NeedRune()
        { return false; }

        public void SetPropertyRune(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property, Rune Rune)
        {

        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            if ((ObjectAtRunic as IDictionary<string, object>).ContainsKey(Property.Name))
            {
                if ((ObjectAtRunic as IDictionary<string, string>)[Property.Name] != "")
                {
                    RuneList RList = (RuneList)(ObjectAtRunic as IDictionary<string, object>)[Property.Name];

                    Type ItemsType = Type.GetType("System." + RList.TypeName);
                    if (ItemsType != null)
                    {
                        var ListOfItems = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ItemsType));

                        foreach (var Item in RList.List)
                        {
                            ListOfItems.Add(Convert.ChangeType(Item, ItemsType));
                        }

                        Property.SetValue(Object, ListOfItems);
                    }
                }
            }
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            if (Property.PropertyType.GetGenericArguments()[0].IsPrimitive || Property.PropertyType.GetGenericArguments()[0] == typeof(String))
            {
                RuneList RList = new RuneList() { List = new List<object>(), TypeName = Property.PropertyType.GetGenericArguments()[0].Name };

                var List = (IList)Property.GetValue(Object, null);
                if (List != null)
                    foreach (var Item in List)
                    {
                        RList.List.Add(Item);
                    }

                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, RList);
            }
            //generic
            else
            {

            }
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

            if (Property.PropertyType.GetGenericArguments()[0].IsPrimitive || Property.PropertyType.GetGenericArguments()[0] == typeof(String))
            {
                IEnumerator EnumA = ListA.GetEnumerator();
                IEnumerator EnumB = ListB.GetEnumerator();

                while ((EnumA.MoveNext()) && (EnumB.MoveNext()))
                {
                    if (EnumA.Current.Equals(EnumB.Current))
                        Result = false;
                    else
                        Result = true;

                }
            }
        }

        public void Dispose()
        { }
    }
}
