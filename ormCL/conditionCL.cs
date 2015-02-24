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

using ConditionsLanguage;

namespace ormCL
{
    //wtf
    public class conditionCL
    {
        public string[] Conditions
        {
            get
            {
                return CL.Split(conditions);
            }
        }
        private string conditions;
        [Obsolete("Please, don't use it! CL rules can changes any version! U can use string[] Conditions with CL.Solve")]
        public List<tokenCL> Tokens = new List<tokenCL>();
        public conditionCL(string Conditions)
        {
            conditions = Conditions;            
        }
    }
}
