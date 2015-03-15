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
            Alchemist = new Transmuter<T>();
        }

        protected void Ids()
        {
            try { var Id = typeof(T).GetProperty(typeof(T).Name + "Id"); }
            catch (Exception ex)
            { throw new Exception("Id not founded!", ex); }
        }

        protected Transmuter<T> Alchemist;

        public T this[int i]
        {
            get { return Alchemist.Get[i]; }
            set { Alchemist.Get[i] = value; }
        }

        public void Add(T Item)
        {
            Alchemist.Add(Item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T t in Alchemist)
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
