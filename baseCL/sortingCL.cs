using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Dynamic;
using System.Xml.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;

namespace ormCL
{


    public class sortingCL
    {
        public List<tokenCL> Sortings = new List<tokenCL>();
        public sortingCL(string Sortings)
        {
            foreach (string Sorting in Sortings.Split(','))
            {
                try
                {
                    this.Sortings.Add(
                        new tokenCL()
                        {
                            Field = Sorting.Split(':')[0],
                            Operation = Sorting.Split(':')[1]
                        });
                }
                catch { }
            }
        }
    }

}
