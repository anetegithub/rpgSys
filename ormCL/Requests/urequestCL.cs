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
    public class urequestCl : requestCL
    {
        public urequestCl(conditionCL Conditions)
        {
            this.Conditions = Conditions;
        }

        public object Object;
    }

}
