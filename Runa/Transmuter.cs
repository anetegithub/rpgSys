using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Collections;

using System.Xml.Linq;
using System.Web;
using System.IO;

using System.Dynamic;

using System.Diagnostics;

namespace RuneFramework
{
    public class Transmuter<T> : IEnumerable<T>
    {
        public Transmuter()
        {
            //Init properties
            foreach (PropertyInfo Property in typeof(T).GetProperties())
            {
                if (Property.PropertyType.IsPrimitive)
                    Primitives.Add(Property);
                else if (Property.PropertyType == typeof(String))
                    Strings.Add(Property);
                else if (Property.PropertyType.IsGenericType)
                    Classes.Add(Property);
                else if (Property.PropertyType.IsEnum)
                    Enums.Add(Property);
            }

            //Init document
            if (Rune.Element == RuneElement.Air)
                PathToFile = HttpContext.Current.Server.MapPath("~/Data/" + typeof(T).Name + ".xml");
            else if (Rune.Element == RuneElement.Earth)
                PathToFile = Directory.GetCurrentDirectory() + "/Data/" + typeof(T).Name + ".xml";

            Magican = new Lazy<RuneMage<T>>(() => new RuneMage<T>(PathToFile));            
        }

        protected string PathToFile;
        protected string Id
        {
            get
            {
                if (!typeof(T).IsEnum)
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

        public void Transmute()
        {
            RuneBook Book = new RuneBook();
            Book.Spells = new List<RuneSpell>();
            Book.Spellage = new List<RuneSpellage>();

            List<T> FromFile = LoadRunicWords;
            for (int i = 0; i < FromFile.Count; i++)
            {
                TransmutePrimitives(FromFile[i], Words[i], Book);

                TransmuteStrings(FromFile[i], Words[i], Book);
            }

            Mage.Update(Book);
        }

        private bool Loaded;
        protected void RunicWordsLoaded()
        {
            if (!Loaded)
            { this.Words = LoadRunicWords; Loaded = true; }
        }
        protected List<T> LoadRunicWords
        {
            get
            {
                List<T> Result=new List<T>();
                var RunicWords = Mage.Select(null);
                if (RunicWords.Root != null)
                {
                    foreach (XElement RunicWord in RunicWords.Root.Elements())
                    {
                        Result.Add
                            (
                                TransmuteFromTablet
                                    (
                                        (Tablet<T>.ToWord(RunicWord) as ExpandoObject)
                                    )
                            );
                    }
                }
                return Result;
            }
        }

        protected XElement TransmuteToTablet(T Item)
        {
            dynamic WordAtRunic = new ExpandoObject();

            using (var Letter = new PrimitiveLetter<T>())
            {
                foreach (var Property in Primitives)
                {
                    Letter.GetProperty(ref WordAtRunic, Item, Property);
                }
            }
            return Tablet<T>.ToRunic(WordAtRunic as ExpandoObject);
        }

        protected T TransmuteFromTablet(dynamic Runic)
        {
            T Item = Activator.CreateInstance<T>();

            using(var Letter = new PrimitiveLetter<T>())
            {
                foreach(var Property in Primitives)
                {
                    Letter.SetProperty(ref Item, (Runic as ExpandoObject),Property);
                }
            }

            return Item;
        }

        protected void AboutLetters()
        { }

        protected List<PropertyInfo> Primitives = new List<PropertyInfo>();
        protected void TransmutePrimitives(T FromFile, T Words, RuneBook Book)
        {
            using (var Letter = new PrimitiveLetter<T>())
            {
                foreach (var Property in Primitives)
                {
                    bool NeedRuneSpell = false;
                    Letter.NeedChanges(out NeedRuneSpell, FromFile[i], Words[i], Property);

                    if (NeedRuneSpell)
                    {
                        Book.Spells.Add(
                                new RuneSpell(Id, "==", typeof(T).GetProperty(Id).GetValue(FromFile[i], null))
                            );
                        Book.Spellage.Add(
                                new RuneSpellage(Property.Name, Property.GetValue(Words[i], null).ToString())
                            );
                    }
                }
            }
        }
        protected List<PropertyInfo> Strings = new List<PropertyInfo>();
        protected void TransmuteStrings(T FromFile, T Words, RuneBook Book)
        {
            using (var Letter = new PrimitiveLetter<T>())
            {
                foreach (var Property in Primitives)
                {
                    bool NeedRuneSpell = false;
                    Letter.NeedChanges(out NeedRuneSpell, FromFile[i], Words[i], Property);

                    if (NeedRuneSpell)
                    {
                        Book.Spells.Add(
                                new RuneSpell(Id, "==", typeof(T).GetProperty(Id).GetValue(FromFile[i], null))
                            );
                        Book.Spellage.Add(
                                new RuneSpellage(Property.Name, Property.GetValue(Words[i], null).ToString())
                            );
                    }
                }
            }
        }
        protected List<PropertyInfo> Classes = new List<PropertyInfo>();
        protected List<PropertyInfo> Enums = new List<PropertyInfo>();

        protected Lazy<RuneMage<T>> Magican;
        private RuneMage<T> ShadowMage { get; set; }
        protected RuneMage<T> Mage
        {
            get
            {
                if (!Magican.IsValueCreated)
                    ShadowMage = Magican.Value;

                return ShadowMage;
            }
        }

        protected List<T> Words = new List<T>();
        protected List<dynamic> WordsOnRunic = new List<dynamic>();

        public List<T> Get
        {
            get
            {
                RunicWordsLoaded();
                return Words;
            }
        }

        public void Set(T Value, int Index)
        {
            Words[Index] = Value;
        }

        public void Add(T Item)
        {
            var MaxId = Mage.SelectMax(this.Id);
            Item.GetType().GetProperty(Id).SetValue(Item, Convert.ChangeType(++MaxId, Item.GetType().GetProperty(Id).PropertyType));

            Mage.Insert(new RuneBook() { Elements = new List<XElement>() { TransmuteToTablet(Item) } });

            Words.Add(Item);
        }

        public void Remove(T Item)
        {
            Mage.Delete(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(Id, "==", (int)typeof(T).GetProperty(Id).GetValue(Item, null)) } });
            this.Words.Remove(Item);
        }

        public void Remove(Int32 Index)
        {
            Mage.Delete(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(Id, "==", (int)typeof(T).GetProperty(Id).GetValue(this.Words[Index], null)) } });
            this.Words.Remove(this.Words[Index]);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T t in Words)
            {
                if (t == null)
                {
                    break;
                }
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}