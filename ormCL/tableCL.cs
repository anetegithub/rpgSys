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


    public class tableCl
    {
        private List<string> paths = new List<string>();
        public tableCl(string TableName)
        {
            paths.Add(TableName);
        }
        /// <summary>
        /// Nodes adds at start, like: Core=File, Node=Sample, result=Sample\File
        /// </summary>
        public string Node
        {
            set
            {
                paths.Add(value);
            }
        }

        public string Path
        {
            get
            {
                string r = "";
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    r = paths[i] + "/" + r;
                }
                return r.Substring(0, r.Length - 1) + ".xml";
            }
        }        
    }
}
