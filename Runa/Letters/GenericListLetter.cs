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
                                    .GetMethod("QueryUniq", new Type[] { typeof(RuneBook) })
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

        public void Dispose()
        { }
    }
}
