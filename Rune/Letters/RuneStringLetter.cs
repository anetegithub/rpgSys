using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace RuneFramework
{
    public class RuneStringLetter<T> : ILetter<T>
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
                                .GetMethod("QueryUniq", new Type[] { typeof(RuneBook) })
                                .Invoke(RuneWord.GetValue(Rune, null),
                                new object[] { new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(IdName, "==", Id) } } });

                            Property.SetValue(Object, Value);
                        }
                    }
                }
            }
        }

        public void SetProperty(ref T Object, dynamic ObjectAtRunic, PropertyInfo Property)
        {
            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                if (Field.Key == Property.Name)
                {
                    RuneString Value = Int32.Parse(Field.Value.ToString());

                    /* Perfomance
                     * 
                     * 2 Items and Id=1:
                     * Foreach : ~ 601-687
                     * Query : ~ 1008-1144
                     * 
                     * 52 Items and Id=50:
                     * Foreach : ~ 1312 - 2006
                     * Query : ~ 1740 - 2417
                     * 
                     * 52 Items and Id=25:
                     * Foreach : ~ 1233 - 1305
                     * Query : ~ 1821 - 2581
                     *
                     */

                    if (typeof(T) != typeof(RuneString))
                    {
                        RuneWord<RuneString> Runum = new RuneWord<RuneString>(Property.Name, null);
                        //var Query = Runum.Query(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", Value.Id.ToString()) } });
                        //Value.Value = Query[0].Value;
                        foreach (var RunumItem in Runum)
                        {
                            if (RunumItem.Id == Value.Id)
                            {
                                Value = new RuneString(RunumItem.Value);
                            }
                        }
                    }

                    Property.SetValue(Object, Value);
                }
            }
        }

        public void GetProperty(ref dynamic ObjectAtRunic, T Object, PropertyInfo Property)
        {
            var Value = Property.GetValue(Object, null);
            if (Value != null)
                (ObjectAtRunic as IDictionary<string, object>).Add(Property.Name, Value.ToString());
        }

        public void Dispose()
        { }
    }
}
