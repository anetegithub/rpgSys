using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ormCL.Attributes
{
    /// <summary>
    /// |DateTimeElementWithoutStringify| |Field0| |Field1| |Field2| |/DateTimeElementWithoutStringify| 
    /// |DateTimeElementStringify| 01.01.0001 0:00:00 |/DateTimeElementStringify|
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class stringifyCLAttribute : Attribute
    {
        public readonly bool Yes = true;
    }
}
