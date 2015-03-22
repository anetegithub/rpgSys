using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace RuneFramework
{
    public class RuneWord<T> : IEnumerable<T>
    {
        public RuneWord(string TableName)
        {
            Ids();
            if (typeof(T) != typeof(RuneString))
                Transmuter = new Transmuter<T>(typeof(T).Name);
            else
                Transmuter = new Transmuter<T>(TableName);

            var f = Transmuter.Get;
        }
        protected void Ids()
        {
            if (typeof(T)!=typeof(RuneString))
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
