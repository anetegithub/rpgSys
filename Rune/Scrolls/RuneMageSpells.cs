using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RuneFramework
{
    public static class RuneMageSpells
    {
        public static int CompareToIn(this string c, object obj)
        {
            string x = Convert.ToString(c);
            string y = Convert.ToString(obj);
            return x.IndexOf(y);
        }

        public static int CompareToLike(this string c, object obj)
        {
            string x = Convert.ToString(c);
            string y = Convert.ToString(obj);

            int together = 0, length = (x.Length > y.Length ? x.Length : y.Length);

            for (int i = 0; i < length; i++)
            {
                char xc = ' ', yc = ' ';
                try { xc = x[i]; }
                catch { }
                try { yc = y[i]; }
                catch { }

                if (xc == yc)
                    together++;
            }

            return together >= length / 2 ? 1 : 0;
        }
              
    }
}
