using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using System.Xml.Linq;

using System.Dynamic;

namespace RuneFramework
{
    public interface IRuneMage<T>
    {
        void Transmute(T FromFile, T Words, RuneBook Book);
        XElement ToTablet(T Item, ref dynamic WordAtRunic);
        T FromTablet(dynamic Runic, ref T Item);
    }

    public class RuneMage<T> : IRuneMage<T>
    {
        public RuneMage(Letter<T> Letters)
        {
            this.SpecificLetter = Letters;
        }

        public List<PropertyInfo> Properties = new List<PropertyInfo>();
        public Rune Rune;

        private Letter<T> SpecificLetter;

        private string Id
        {
            get
            {
                if (typeof(T) != typeof(RuneString))
                {
                    var Id = typeof(T).GetProperty("Id");
                    if (Id == null)
                        Id = typeof(T).GetProperty(typeof(T).Name + "Id");
                    if (Id == null)
                        throw new Exception(typeof(T).Name + " class : Id not found!");
                    return Id.Name;
                }
                else
                    return "Id";
            }
        }

        public void Transmute(T FromFile, T Words, RuneBook Book)
        {
            using (var Letter = SpecificLetter)
            {
                foreach (var Property in Properties)
                {
                    bool NeedRuneSpell = false;
                    
                    Letter.NeedChanges(out NeedRuneSpell, FromFile, Words, Property);

                    if (NeedRuneSpell)
                    {
                        Book.Spells.Add(
                                new RuneSpell(Id, "==", typeof(T).GetProperty(Id).GetValue(FromFile, null))
                            );
                        var Value = Property.GetValue(Words, null);

                        var StrId = "0";
                        var ObjId = Value.GetType().GetProperty("Id");
                        if (ObjId == null)
                            StrId = Value.GetType().Name + "Id";
                        else
                            StrId = "Id";

                        Book.Spellage.Add(
                                new RuneSpellage(Property.Name, Value.GetType().GetProperty(StrId).GetValue(Value, null).ToString())
                            );
                    }
                }
            }
        }

        public XElement ToTablet(T Item, ref dynamic WordAtRunic)
        {
            using (var Letter = SpecificLetter)
                foreach (var Property in Properties)
                    Letter.GetProperty(ref WordAtRunic, Item, Property);

            return Tablet<T>.ToRunic(WordAtRunic as ExpandoObject);
        }

        public T FromTablet(dynamic Runic, ref T Item)
        {
            using (var Letter = SpecificLetter)
                foreach (var Property in Properties)
                {
                    if (!Letter.NeedRune())
                        Letter.SetProperty(ref Item, (Runic as ExpandoObject), Property);
                    else
                        Letter.SetPropertyRune(ref Item, (Runic as ExpandoObject), Property, Rune);
                }

            return Item;
        }
    }
}