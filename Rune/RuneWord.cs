using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Xml.Linq;

namespace RuneFramework
{
    public class RuneWord<T> : IEnumerable<T> where T : class
    {
        public RuneWord(string TableName, Rune Rune)
        {
            Ids();
            if (typeof(T) != typeof(RuneString))
                Transmuter = new Transmuter<T>(typeof(T).Name);
            else
                Transmuter = new Transmuter<T>(TableName);

            Transmuter.Rune = Rune;
        }
        protected void Ids()
        {
            if (typeof(T) != typeof(RuneString))
            {
                var Id = typeof(T).GetProperty("Id");
                if (Id == null)
                    Id = typeof(T).GetProperty(typeof(T).Name + "Id");
                if (Id == null)
                    throw new Exception(typeof(T).Name + " class : Id not found!");
            }
        }

        protected Transmuter<T> Transmuter;
        public void WriteRuneWord()
        {
            Transmuter.Transmute();
        }

        public List<T> Query(RuneBook Book)
        {
            return Transmuter.RealiseQuery(Book);
        }
        public object QueryUniq(RuneBook Book)
        {
            var QueryResult = Transmuter.RealiseQuery(Book);
            if (QueryResult.Count != 0)
                return Transmuter.RealiseQuery(Book)[0];
            else
                return null;
        }
        public object QueryUniq(String Field, String Operator, Object Value)
        {
            RuneSpell Spell = new RuneSpell(Field, Operator, Value);
            RuneBook Book = new RuneBook() { Spells = new List<RuneSpell>() { Spell } };
            var QueryResult = Transmuter.RealiseQuery(Book);
            if (QueryResult.Count != 0)
                return Transmuter.RealiseQuery(Book)[0];
            else
                return null;
        }
        public T QueryUniqSafe(String Field,String Operator,Object Value)
        {
            RuneSpell Spell = new RuneSpell(Field, Operator, Value);
            RuneBook Book = new RuneBook() { Spells = new List<RuneSpell>() { Spell } };
            var QueryResult = Transmuter.RealiseQuery(Book);
            if (QueryResult.Count != 0)
                return Transmuter.RealiseQuery(Book)[0];
            else
                return (new Object() as T);
        }

        public List<T> Reference(SimpleRuneBook Book)
        {
            if (Book.Spells == null)
                return new List<T>();
            if (Book.Spells.Count == 0)
                return new List<T>();

            var LinqQuery = (from a in Transmuter.Get where Book.Spells[0].Spell(a) select a).ToList();

            foreach (SimpleRuneSpell RuneSp in Book.Spells.Skip(1))
                LinqQuery.Where(x => RuneSp.Spell(x));

            if (LinqQuery.Count != 0)
                return LinqQuery;
            else
                return new List<T>();
        }
        public List<T> Reference(String Field, String Operator, Object Value)
        {
            SimpleRuneSpell Spell = new SimpleRuneSpell(Field, Operator, Value);
            var LinqQuery = (from a in Transmuter.Get where Spell.Spell(a) select a).ToList();
            if (LinqQuery.Count != 0)
                return LinqQuery;
            else
                return new List<T>();
        }

        public T ReferenceUniq(String Field, String Operator, Object Value)
        {
            var ReferenceList = this.Reference(Field, Operator, Value);
            if (ReferenceList.Count != 0)
                return ReferenceList[0];
            else
                throw new ArgumentException();
        }
        public T ReferenceUniq(SimpleRuneBook Book)
        {
            var ReferenceList = this.Reference(Book);
            if (ReferenceList.Count != 0)
                return ReferenceList[0];
            else
                throw new ArgumentException();
        }

        public T this[int i]
        {
            get
            {
                try
                {
                    return Transmuter.Get[i];
                }
                catch (ArgumentOutOfRangeException Ex)
                {
                    throw new ArgumentOutOfRangeException("Index out of range", Ex);
                }
            }
            set { Transmuter.Get[i] = value; }
        }
        public void Add(T Item)
        {
            Transmuter.Add(Item);
        }
        public void Remove(T Item)
        {
            Transmuter.Remove(Item);
        }
        public void Remove(Int32 Index)
        {
            Transmuter.Remove(Index);
        }
        public T Find(Predicate<T> match)
        {
            return Transmuter.Find(match);
        }
        public int IndexOf(T Item)
        { return Transmuter.IndexOf(Item); }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T t in Transmuter)
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