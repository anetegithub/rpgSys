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
        public Transmuter(String TableName)
        {
            //Init Shamans
            Primitives = new RuneMage<T>(new PrimitiveLetter<T>());
            Strings = new RuneMage<T>(new StringLetter<T>());
            RuneStrings = new RuneMage<T>(new RuneStringLetter<T>());
            Classes = new RuneMage<T>(new ClassLetter<T>());
            Lists = new RuneMage<T>(new ListsLetter<T>());

            //Init properties
            foreach (PropertyInfo Property in typeof(T).GetProperties())
            {
                if (Property.PropertyType.IsPrimitive)
                    Primitives.Properties.Add(Property);

                else if (Property.PropertyType == typeof(RuneString))
                    RuneStrings.Properties.Add(Property);

                else if (Property.PropertyType == typeof(String))
                    Strings.Properties.Add(Property);

                else if (Property.PropertyType.IsClass
                                                && Property.PropertyType != typeof(RuneString)
                                                && Property.PropertyType != typeof(String)
                                                && Property.PropertyType.GetInterface("IList") == null)
                    Classes.Properties.Add(Property);
                else if (Property.PropertyType.GetInterface("IList") != null)
                {
                    Lists.Properties.Add(Property);
                }
            }

            //Init document
            if (Rune.Element == RuneElement.Air)
                PathToFile = HttpContext.Current.Server.MapPath("~/Data/" + TableName + ".xml");
            else if (Rune.Element == RuneElement.Earth)
                PathToFile = Directory.GetCurrentDirectory() + "/Data/" + TableName + ".xml";

            Shamanism = new Lazy<RuneShaman<T>>(() => new RuneShaman<T>(PathToFile));
        }

        public Rune Rune;

        protected string PathToFile;
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

        public void Transmute()
        {
            RuneBook Book = new RuneBook();
            Book.Spells = new List<RuneSpell>();
            Book.Spellage = new List<RuneSpellage>();

            RunicWordsLoaded();

            List<T> FromFile = LoadRunicWords;
            for (int i = 0; i < FromFile.Count; i++)
            {
                Primitives.Transmute(FromFile[i], Words[i], Book);

                Strings.Transmute(FromFile[i], Words[i], Book);

                if (RuneStrings.Rune == null)
                    RuneStrings.Rune = Rune;
                RuneStrings.Transmute(FromFile[i], Words[i], Book);
                
                if (Classes.Rune == null)
                    Classes.Rune = Rune;
                Classes.Transmute(FromFile[i], Words[i], Book);

                Lists.Transmute(FromFile[i], Words[i], Book);
            }

            Shaman.Update(Book);

            Loaded = false;
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
                List<T> Result = new List<T>();
                var RunicWords = Shaman.SelectStream(null);
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
        public List<T> RealiseQuery(RuneBook Book)
        {
            List<T> Result = new List<T>();
            var RunicWords = Shaman.SelectStream(Book);
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

        protected XElement TransmuteToTablet(T Item)
        {
            dynamic WordAtRunic = new ExpandoObject();

            Primitives.ToTablet(Item, ref WordAtRunic);

            Strings.ToTablet(Item, ref WordAtRunic);

            if (RuneStrings.Rune == null)
                RuneStrings.Rune = Rune;
            RuneStrings.ToTablet(Item, ref WordAtRunic);

            if (Classes.Rune == null)
                Classes.Rune = Rune;
            Classes.ToTablet(Item, ref WordAtRunic);

            Lists.ToTablet(Item, ref WordAtRunic);

            return Tablet<T>.ToRunic(WordAtRunic as ExpandoObject);
        }
        protected T TransmuteFromTablet(dynamic Runic)
        {
            T Item = Activator.CreateInstance<T>();

            Primitives.FromTablet(Runic as ExpandoObject, ref Item);

            Strings.FromTablet(Runic as ExpandoObject, ref Item);

            if (RuneStrings.Rune == null)
                RuneStrings.Rune = Rune;
            RuneStrings.FromTablet(Runic as ExpandoObject, ref Item);

            if (Classes.Rune == null)
                Classes.Rune = Rune;
            Classes.FromTablet(Runic as ExpandoObject, ref Item);

            Lists.FromTablet(Runic as ExpandoObject, ref Item);

            return Item;
        }

        protected RuneMage<T> Primitives { get; set; }
        protected RuneMage<T> Strings { get; set; }
        protected RuneMage<T> RuneStrings { get; set; }
        protected RuneMage<T> Classes { get; set; }
        protected RuneMage<T> Lists { get; set; }


        protected Lazy<RuneShaman<T>> Shamanism;
        private RuneShaman<T> ShadowShaman { get; set; }
        protected RuneShaman<T> Shaman
        {
            get
            {
                if (!Shamanism.IsValueCreated)
                    ShadowShaman = Shamanism.Value;

                return ShadowShaman;
            }
        }

        protected List<T> Words = new List<T>();
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
            var MaxId = Shaman.SelectMaxStream(this.Id);
            Item.GetType().GetProperty(Id).SetValue(Item, Convert.ChangeType(++MaxId, Item.GetType().GetProperty(Id).PropertyType));

            Shaman.Insert(new RuneBook() { Elements = new List<XElement>() { TransmuteToTablet(Item) } });

            Words.Add(Item);
        }
        public void Remove(T Item)
        {
            Shaman.Delete(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(Id, "==", (int)typeof(T).GetProperty(Id).GetValue(Item, null)) } });
            this.Words.Remove(Item);
        }
        public void Remove(Int32 Index)
        {
            Shaman.Delete(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell(Id, "==", (int)typeof(T).GetProperty(Id).GetValue(this.Words[Index], null)) } });
            this.Words.Remove(this.Words[Index]);
        }

        public IEnumerator<T> GetEnumerator()
        {
            RunicWordsLoaded();
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