using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

using System.Xml;

namespace RuneFramework
{
    public class RuneShaman<T>
    {
        public RuneShaman(string Path)
        {
            this.Path = Path;
        }

        public String Path = "";

        protected Lazy<XDocument> LazyDocument;
        protected XDocument Document;
        protected static object Key = new object();

        protected void PrepeareSpell()
        {
            if (LazyDocument == null)
                LazyDocument = new Lazy<XDocument>(() => XDocument.Load(Path));
            if (!LazyDocument.IsValueCreated)
                Document = LazyDocument.Value;
            if (Path == "")
                throw new Exception("Path to file not found!");
        }
        protected string Id
        {
            get
            {
                if (typeof(T)!=typeof(RuneString))
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

        [Obsolete("Slow")]
        public XDocument Select(RuneBook Query)
        {
            if (Query.Spells == null || Query.Spells.Count == 0)
                Query.Spells = new List<RuneSpell>() { new RuneSpell(Id, "!=", 0) };

            PrepeareSpell();

            var some = (
                from Item
                in Document.Root.Elements()
                where Query.Spells[0].Spell(Item)
                select Item
                );

            foreach (RuneSpell RuneSp in Query.Spells.Skip(1))
                some.Where(x => RuneSp.Spell(x));

            return new XDocument(some);
        }

        public XDocument SelectStream(RuneBook Query)
        {
            if (Query == null)
                Query = new RuneBook();
            if (Query.Spells == null || Query.Spells.Count == 0)
                Query.Spells = new List<RuneSpell>() { new RuneSpell(Id, "!=", 0) };

            var some = (
                from Item
                in StreamRootChildDoc
                where Query.Spells[0].Spell(Item)
                select Item
                );

            foreach (RuneSpell RuneSp in Query.Spells.Skip(1))
                some.Where(x => RuneSp.Spell(x));

            return new XDocument(new XElement("Root", some));
        }

        public XDocument Delete(RuneBook Query)
        {
            if (Query.Spells == null || Query.Spells.Count == 0)
                Query.Spells = new List<RuneSpell>() { new RuneSpell(Id, "!=", 0) };

            PrepeareSpell();

            var some = Document.Root.Elements(typeof(T).Name).Where(x => Query.Spells[0].Spell(x));
            
            foreach (RuneSpell RuneSp in Query.Spells.Skip(1))
                some.Where(x => RuneSp.Spell(x));

            some.Remove();

            lock (Key) { Document.Save(Path); }

            return new XDocument();
        }

        public XDocument Update(RuneBook Query)
        {
            var Doc = SelectStream(null);
            var XCollection = Doc.Elements().ToList();
            var Collection = XCollection[0].Elements().ToList();

            for (int i = 0; i < Collection.Count; i++)
            {
                bool NeedToBeChanged = false;

                foreach (RuneSpell RuneSp in Query.Spells)
                    NeedToBeChanged = RuneSp.Spell(Collection[i]);

                if (NeedToBeChanged)
                {
                    XElement XEl = Collection[i];
                    foreach (RuneSpellage Spellage in Query.Spellage)
                    {
                        Spellage.SetValue(ref XEl);
                    }
                    Collection[i] = XEl;
                }
            }
            if (Collection.ToList().Count != 0)
            {
                lock (Key) { Document = new XDocument(new XElement(typeof(T).Name + "s", Collection)); Document.Save(Path); }
            }

            return new XDocument(Doc.Elements());
        }

        public XDocument Insert(RuneBook Query)
        {
            PrepeareSpell();

            foreach (XElement XEl in Query.Elements)
                Document.Root.Add(XEl);

            lock (Key) { Document.Save(Path); }

            return new XDocument(Query.Elements);
        }

        [Obsolete("Slow")]
        public double SelectMax(String ClassName, String FieldName)
        {
            PrepeareSpell();

            if (Document.Root.Elements().Count() != 0)
            {
                var some = Document.Root.Elements(ClassName).Max(c => (double)c.Element(FieldName));
                return some;
            }
            else
                return 0;
        }

        public double SelectMaxStream(String FieldName)
        {
            PrepeareSpell();

            if (Document.Root.Elements().Count() != 0)
            {
                var some = StreamRootChildDoc.Max(c => (double)c.Element(FieldName));
                return some;
            }
            else
                return 0;
        }
        protected IEnumerable<XElement> StreamRootChildDoc
        {
            get
            {
                using (XmlReader reader = XmlReader.Create(Path))
                {
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                {
                                    XElement el = XElement.ReadFrom(reader) as XElement;
                                    if (el != null)
                                        yield return el;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    //public sealed class Singleton<T> where T : class, new()
    //{
    //    Singleton()
    //    { }

    //    public static T Instance
    //    {
    //        get
    //        {
    //            return Nested.instance;
    //        }
    //    }

    //    class Nested
    //    {
    //        static Nested()
    //        { }

    //        internal static readonly T instance = new T();
    //    }
    //}
}