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
    public class drequestCl : requestCL
    {
        public drequestCl(conditionCL Conditions)
        {
            this.Conditions = Conditions;
        }

        public object Object;
    }

}
