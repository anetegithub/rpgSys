using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ormCL.Attributes
{
    /// <summary>
    /// |ChangedName| ... |/ChangedName|
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class nameCLAttribute : Attribute
    {
        public readonly string Name;
        public nameCLAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
