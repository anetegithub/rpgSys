using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace RuneFramework
{
    public class RuneMage<T>
    {
        public RuneMage(String Path)
        {
            this.Path = Path;
            LazyDocument = new Lazy<XDocument>(() => XDocument.Load(Path));
        }

        protected String Path = "";

        protected Lazy<XDocument> LazyDocument;
        protected XDocument Document;
        protected static object Key = new object();

        protected void PrepeareSpell()
        {
            if (!LazyDocument.IsValueCreated)
                Document = LazyDocument.Value;
            if (Path == "")
                throw new Exception("Path to file not found!");
        }
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

        public XDocument Select(RuneBook Query)
        {
            if (Query.Spells == null || Query.Spells.Count == 0)
                Query.Spells = new List<RuneSpell>() { new RuneSpell(Id, "!=", 0) };

            PrepeareSpell();


            var some = (
                from Item
                in Document.Root.Descendants()
                where Query.Spells[0].Spell(Item)
                select Item
                );

            foreach (RuneSpell RuneSp in Query.Spells.Skip(1))
                some.Where(x => RuneSp.Spell(x));

            return new XDocument(some);
        }

        public XDocument Delete(RuneBook Query)
        {
            if (Query.Spells == null || Query.Spells.Count == 0)
                Query.Spells = new List<RuneSpell>() { new RuneSpell(Id, "!=", 0) };

            PrepeareSpell();


            var some = (
                from Item
                in Document.Root.Descendants()
                where Query.Spells[0].Spell(Item)
                select Item
                );

            foreach (RuneSpell RuneSp in Query.Spells.Skip(1))
                some.Where(x => RuneSp.Spell(x));

            some.Remove();

            lock (Key) { Document.Save(Path); }

            return new XDocument();
        }

        public XDocument Update(RuneBook Query)
        {
            PrepeareSpell();

            var Doc = Select(Query);
            var Collection = Doc.Elements().ToList();

            for (int i = 0; i < Collection.Count; i++)
            {
                XElement XEl = Collection[i];
                foreach (RuneSpellage Spellage in Query.Spellage)
                {
                    Spellage.SetValue(ref XEl);
                }
                Collection[i] = XEl;
            }

            lock (Key) { Document.Save(Path); }

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
    }
}