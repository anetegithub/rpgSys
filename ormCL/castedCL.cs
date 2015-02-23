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
    public class castedCL<T>
    {
        private static readonly string wtf = "wtf";
        private U CreateObject<U>()
        {
            return (U)Activator.CreateInstance(typeof(U));
        }
        private Type GetListType(Type T)
        {
            var Ltype = T;
            foreach (Type interfaceType in Ltype.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition()
                    == typeof(IList<>))
                {
                    return Ltype.GetGenericArguments()[0];
                }
            }
            return typeof(Nullable);
        }
        private List<T> Result = new List<T>();
        private castedCL()
        { }
        protected responseCL response;
        public castedCL(responseCL Response)
        {
            response = Response;
            MethodInfo method = typeof(castedCL<T>).GetMethod("CastCollection");
            MethodInfo generic = method.MakeGenericMethod(typeof(T));
            ParameterInfo[] parameters = generic.GetParameters();
            object classInstance = new castedCL<T>();
            object[] parametersArray = new object[] { Response.Response };
            Result = (List<T>)generic.Invoke(classInstance, parametersArray);
        }

        public V Cast<V>(List<dynamic> Object)
        {
            var o = CreateObject<V>();
            foreach (var DField in Object)
            {
                foreach (var Property in typeof(V).GetProperties())
                {
                    foreach (var dWhat in DField)
                    {
                        if (Property.Name == dWhat.Key)
                        {
                            var pType = Property.PropertyType;
                            if (dWhat.Value.GetType() == typeof(List<dynamic>))
                            {
                                MethodInfo method = typeof(castedCL<T>).GetMethod("CastCollection");
                                var CollectionType = GetListType(Property.PropertyType);
                                MethodInfo generic = method.MakeGenericMethod(CollectionType);
                                object classInstance = new castedCL<T>();
                                object[] parametersArray = new object[] { dWhat.Value };
                                var CastedCollection = generic.Invoke(classInstance, parametersArray);
                                o.GetType()
                                    .GetProperty(Property.Name)
                                    .SetValue(
                                    o,
                                    CastedCollection);
                            }
                            else
                            {
                                o.GetType()
                                    .GetProperty(Property.Name)
                                    .SetValue(
                                    o,
                                    Convert.ChangeType(dWhat.Value, Property.PropertyType));
                            }
                        }
                    }
                }
            }

            return o;
        }
        public List<O> CastCollection<O>(List<dynamic> Collection)
        {
            List<O> List = new List<O>();
            foreach (dynamic CollectionItem in Collection)
            {
                List.Add(Cast<O>(new List<dynamic>() { CollectionItem }));
            }
            return List;
        }

        public List<T> ToList()
        {
            return Result;
        }
        public T UniqueResult()
        {
            return Result[0];
        }

        public castedCL<T> Sort(sortingCL Sortings)
        {
            object Temp = new object();

            if (Sortings.Sortings.Count != 0)
            {
                if (Sortings.Sortings[0].Operation == "Desc")
                {
                    Temp = Result.OrderByDescending(x => typeof(T).GetProperty(Sortings.Sortings[0].Field).GetValue(x, null));
                }
                else
                {
                    Temp = Result.OrderBy(x => typeof(T).GetProperty(Sortings.Sortings[0].Field).GetValue(x, null));
                }
            }

            if (Sortings.Sortings.Count > 1)
            {
                for (int i = 1; i < Sortings.Sortings.Count; i++)
                {
                    var v2 = i;
                    if (Sortings.Sortings[v2].Operation == "Desc")
                    {
                        Temp = (Temp as IOrderedEnumerable<T>).ThenByDescending(x => typeof(T).GetProperty(Sortings.Sortings[v2].Field).GetValue(x, null));
                    }
                    else
                    {
                        Temp = (Temp as IOrderedEnumerable<T>).ThenBy(x => typeof(T).GetProperty(Sortings.Sortings[v2].Field).GetValue(x, null));
                    }
                }
            }
            Result = (Temp as IOrderedEnumerable<T>).ToList<T>();

            return this;
        }
        public castedCL<T> Limit(Int32 Count)
        {
            Result = Result.Take(Count).ToList<T>();
            return this;
        }
        public castedCL<T> Filter()
        {
            this.Filter(response.Conditions);
            return this;
        }
        public castedCL<T> Filter(conditionCL Conditions)
        {
            List<T> Filtered = new List<T>();
            foreach (T Row in Result)
            {
                bool Add = false;
                foreach (string Condition in Conditions.Conditions)
                {                    
                    Add = CL.Solve(Row, Condition);
                }
                /*foreach (tokenCl Condition in Conditions.Tokens)
                {
                    Add = CL.Satisfy(Row, Condition.Field, Condition.Operation, Condition.Value);
                }*/
                if (Add)
                    Filtered.Add(Row);
            }
            Result = Filtered;
            return this;
        }
    }
}
