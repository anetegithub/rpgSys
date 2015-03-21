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
        XElement ToTablet(T Item);
        T FromTablet(dynamic Runic);
    }

    public class RuneMage<T> : IRuneMage<T>
    {
        public RuneMage(Letter<T> Letters)
        {
            this.SpecificLetter = Letters;
        }

        public List<PropertyInfo> Properties { get; set; }

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
                        Book.Spellage.Add(
                                new RuneSpellage(Property.Name, Property.GetValue(Words, null).ToString())
                            );
                    }
                }
            }
        }

        /////////////////////////////// TODO:
        ///////// ADD REFERENCE CUZ NEED ONE OBJECT BUT MUCH FIELD TYPES


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public XElement ToTablet(T Item)
        {
            dynamic WordAtRunic = new ExpandoObject();

            using (var Letter = SpecificLetter)
                foreach (var Property in Properties)
                    Letter.GetProperty(ref WordAtRunic, Item, Property);

            return Tablet<T>.ToRunic(WordAtRunic as ExpandoObject);
        }

        public T FromTablet(dynamic Runic)
        {
            T Item = Activator.CreateInstance<T>();

            using (var Letter = new PrimitiveLetter<T>())
                foreach (var Property in Properties)
                    Letter.SetProperty(ref Item, (Runic as ExpandoObject), Property);

            return Item;
        }
    }
}