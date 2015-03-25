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

        public void Dispose()
        { }
    }
}
