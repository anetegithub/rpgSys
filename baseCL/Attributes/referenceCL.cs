using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ormCL.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class referenceCLAttribute : Attribute
    {
        public readonly string Table;
        public referenceCLAttribute(string Table)
        {
            this.Table = Table;
        }
    }
}
