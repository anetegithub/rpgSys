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

        public void Dispose()
        { }
    }
}
