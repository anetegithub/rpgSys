using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ormCL.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class outerCLAttribute : Attribute
    {
        public readonly string Key;
        public outerCLAttribute(string Key)
        {
            this.Key = Key;
        }
    }
}
