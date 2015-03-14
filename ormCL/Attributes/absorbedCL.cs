using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ormCL.Attributes
{
    /// <summary>
    /// |Element| AbsorbedFieldValue |/Element|
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class absorbedCLAttribute : Attribute
    {
        public readonly bool Yes = true;           
    }
}
