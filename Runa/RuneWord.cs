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
        public RuneWord()
        {
            Ids();
            Transmuter = new Transmuter<T>();
        }

        protected void Ids()
        {
            if (!typeof(T).IsEnum)
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

        public T this[int i]
        {
            get { return Transmuter.Get[i]; }
            set { Transmuter.Get[i] = value; }
        }

        public void Add(T Item)
        {
            Transmuter.Add(Item);
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
