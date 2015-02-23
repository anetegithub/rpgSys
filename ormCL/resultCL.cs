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

    public class resultCL
    {
        private resultCL(responseCL Response)
        {
            response = Response;
        }
        private responseCL response;
        public responseCL Response
        { get { return response; } }
        public castedCL<T> Cast<T>()
        {
            return new castedCL<T>(response);
        }

        public static resultCL ConnectResponse(responseCL Response)
        {
            resultCL response = new resultCL(Response);
            return response;
        }
    }
}
