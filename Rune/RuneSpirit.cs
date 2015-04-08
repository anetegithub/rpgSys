using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace RuneFramework
{
    public class RuneSpirit<T>
    {
        protected string Id
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

        public XDocument Select(RuneBook Query, String Path)
        {
            if (Query == null)
                Query = new RuneBook();
            if (Query.Spells == null || Query.Spells.Count == 0)
                Query.Spells = new List<RuneSpell>() { new RuneSpell(Id, "!=", 0) };

            RuneMaster RuneMaster = Master.Value;
            RuneMaster.Path = Path;

            var some = (
                from Item
                in RuneMaster.Elements
                where Query.Spells[0].Spell(Item)
                select Item
                );

            RuneMaster.Close();

            foreach (RuneSpell RuneSp in Query.Spells.Skip(1))
                some.Where(x => RuneSp.Spell(x));


            return new XDocument(new XElement("Root", some));
        }

        public XDocument Delete(RuneBook Query, String Path)
        {
            if (Query.Spells == null || Query.Spells.Count == 0)
                Query.Spells = new List<RuneSpell>() { new RuneSpell(Id, "!=", 0) };

            RuneMaster RuneMaster = Master.Value;
            RuneMaster.Path = Path;

            var some = RuneMaster.Document.Root.Elements(typeof(T).Name).Where(x => Query.Spells[0].Spell(x));

            foreach (RuneSpell RuneSp in Query.Spells.Skip(1))
                some.Where(x => RuneSp.Spell(x));

            some.Remove();

            RuneMaster.AddToLine(() =>
            {
                RuneMaster.Document.Save(Path);
            });
            RuneMaster.Close();

            return new XDocument();
        }

        public XDocument Update(RuneBook Query, String Path)
        {
            List<XElement> Elements = Select(null, Path).Elements().ToList()[0].Elements().ToList();
            for (int i = 0; i < Elements.Count; i++)
            {
                XElement CurrentElement = Elements[i];

                IEnumerator SpellEnum = Query.Spells.GetEnumerator();
                IEnumerator SpellageEnum = Query.Spellage.GetEnumerator();

                while (SpellEnum.MoveNext() && SpellageEnum.MoveNext())
                    if ((SpellEnum.Current as RuneSpell).Spell(CurrentElement))
                        (SpellageEnum.Current as RuneSpellage).SetValue(ref CurrentElement);

                if (Elements.Count() != 0)
                {
                    RuneMaster RuneMaster = Master.Value;
                    RuneMaster.Path = Path;

                    RuneMaster.ReWrite(new XElement(typeof(T).Name + 's', Elements));
                    RuneMaster.Close();
                }
            }

            return new XDocument(new XElement("Result", "True"));
        }

        public XDocument Insert(RuneBook Query, String Path)
        {
            RuneMaster RuneMaster = Master.Value;
            RuneMaster.Path = Path;

            foreach (XElement XEl in Query.Elements)
                RuneMaster.Document.Root.Add(XEl);

            RuneMaster.AddToLine(() =>
            {
                RuneMaster.Document.Save(Path);
            });
            RuneMaster.Close();

            return new XDocument(Query.Elements);
        }

        public Double LastId(String FieldName, String Path)
        {
            RuneMaster RuneMaster = Master.Value;
            RuneMaster.Path = Path;

            if (RuneMaster.Document.Root.Elements().Count() != 0)
            {
                var some = RuneMaster.Elements.Max(c => (double)c.Element(FieldName));
                return some;
            }
            else
                return 0;

            RuneMaster.Close();
        }

        private Lazy<RuneMaster> Master = new Lazy<RuneMaster>();
    }
}
