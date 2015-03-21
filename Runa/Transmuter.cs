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
            //Init Shamans
            Primitives = new RuneMage<T>(new PrimitiveLetter<T>());
            Strings = new RuneMage<T>(new StringLetter<T>());
            RuneStrings = new RuneMage<T>(new RuneStringLetter<T>());
            Classes=new RuneMage<T>(new PrimitiveLetter<T>());

            //Init properties
            foreach (PropertyInfo Property in typeof(T).GetProperties())
            {
                if (Property.PropertyType.IsPrimitive)
                    Primitives.Properties.Add(Property);

                else if (Property.PropertyType == typeof(String))
                    Strings.Properties.Add(Property);

                else if (Property.PropertyType.IsGenericType)
                    Classes.Properties.Add(Property);

                else if (Property.PropertyType == typeof(RuneString))
                    RuneStrings.Properties.Add(Property);
            }

            //Init document
            if (Rune.Element == RuneElement.Air)
                PathToFile = HttpContext.Current.Server.MapPath("~/Data/" + typeof(T).Name + ".xml");
            else if (Rune.Element == RuneElement.Earth)
                PathToFile = Directory.GetCurrentDirectory() + "/Data/" + typeof(T).Name + ".xml";

            Magican = new Lazy<RuneMage<T>>(() => new RuneMage<T>(PathToFile));            
        }
                       
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

            List<T> FromFile = LoadRunicWords;
            for (int i = 0; i < FromFile.Count; i++)
            {
                Primitives.Transmute(FromFile[i], Words[i], Book);

                Strings.Transmute(FromFile[i], Words[i], Book);

                RuneStrings.Transmute(FromFile[i], Words[i], Book);
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
                foreach (var Property in Primitives.Properties)
                    Letter.GetProperty(ref WordAtRunic, Item, Property);

            using (var Letter = new StringLetter<T>())
                foreach (var Property in Strings.Properties)
                    Letter.GetProperty(ref WordAtRunic, Item, Property);

            using (var Letter = new RuneStringLetter<T>())
                foreach (var Property in RuneStrings)
                    Letter.GetProperty(ref WordAtRunic, Item, Property);

            return Tablet<T>.ToRunic(WordAtRunic as ExpandoObject);
        }

        protected T TransmuteFromTablet(dynamic Runic)
        {
            T Item = Activator.CreateInstance<T>();

            using(var Letter = new PrimitiveLetter<T>())
                foreach(var Property in Primitives)
                    Letter.SetProperty(ref Item, (Runic as ExpandoObject),Property);

            using (var Letter = new StringLetter<T>())
                foreach (var Property in Strings)
                    Letter.SetProperty(ref Item, (Runic as ExpandoObject), Property);

            using (var Letter = new RuneStringLetter<T>())
                foreach (var Property in RuneStrings)
                    Letter.SetProperty(ref Item, (Runic as ExpandoObject), Property);

            return Item;
        }

        protected void AboutLetters()
        { }

        protected RuneMage<T> Primitives { get; set; }
        protected RuneMage<T> Strings { get; set; }
        protected RuneMage<T> RuneStrings { get; set; }
        protected RuneMage<T> Classes {get; set;}
        

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