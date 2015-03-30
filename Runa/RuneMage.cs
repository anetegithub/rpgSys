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
        public RuneMage(ILetter<T> Letters)
        {
            this.SpecificLetter = Letters;
        }

        public List<PropertyInfo> Properties = new List<PropertyInfo>();
        public Rune Rune;

        private ILetter<T> SpecificLetter;

        private string Id(Type T)
        {
            if (T != typeof(RuneString))
            {
                var Id = T.GetProperty("Id");
                if (Id == null)
                    Id = T.GetProperty(typeof(T).Name + "Id");
                if (Id == null)
                    return "null";
                return Id.Name;
            }
            else
                return "Id";
        }

        public void Transmute(T FromFile, T Words, RuneBook Book)
        {
            using (var Letter = SpecificLetter)
            {
                foreach (var Property in Properties)
                {
                    Object AValue = Property.GetValue(FromFile, null);
                    Object BValue = Property.GetValue(Words, null);

                    if (!RuneComparer.IsEqual(AValue, BValue))
                    {
                        string PrimaryId = Id(typeof(T));
                        Book.Spells.Add(new RuneSpell(PrimaryId, "==", typeof(T).GetProperty(PrimaryId).GetValue(FromFile, null)));

                        string InnerId = Id(BValue.GetType());

                        if (BValue != null)
                        {
                            if (BValue.GetType().IsPrimitive || BValue.GetType() == typeof(String))
                                Book.Spellage.Add(new RuneSpellage(Property.Name, BValue));
                            else if (BValue.GetType().GetInterface("IList") == null)
                                Book.Spellage.Add(new RuneSpellage(Property.Name, BValue.GetType().GetProperty(InnerId).GetValue(BValue, null)));
                            else
                            {
                                dynamic Reference = new ExpandoObject();
                                var TInstance = (T)Activator.CreateInstance(typeof(T));
                                Property.SetValue(TInstance, BValue);
                                var XElementValue = ToTablet(TInstance, ref Reference);
                                Book.Spellage.Add(new RuneSpellage(Property.Name, XElementValue.Element(Property.Name)));
                            }
                        }
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