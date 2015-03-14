using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ormCL.Attributes
{
    /// <summary>
    /// |Element| |ReferenceElement| 1 |/ReferenceElement| |/Element|
    /// |ReferenceElement OuterAttribute="1"| ... |/ReferenceElement|
    /// </summary>
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
