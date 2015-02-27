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

using ormCL.Attributes;

using ormCL.SpecialTypes;

namespace ormCL
{
    //wtf
    public class castedCL<T>
    {
        private U CreateObject<U>()
        {
            if (typeof(U) == typeof(string)) return ((U)("" as object));
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
        public string dbPath;
        public castedCL(responseCL Response)
        {
            dbPath = Response.dbPath;
            response = Response;
            MethodInfo method = typeof(castedCL<T>).GetMethod("CastCollection");
            MethodInfo generic = method.MakeGenericMethod(typeof(T));
            ParameterInfo[] parameters = generic.GetParameters();
            object classInstance = new castedCL<T>();
            (classInstance as castedCL<T>).dbPath = this.dbPath;
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
                        string outerField = "Id";
                        string table = "";
                        bool reference = false;
                        bool absorbed = false;
                        string name = Property.Name;

                        CheckAttributes(Property, ref name, ref absorbed, ref reference, ref table, ref outerField);

                        if (name == dWhat.Key || (absorbed && dWhat.Key == "AbsorbedValue"))
                        {
                            var pType = Property.PropertyType;
                            if (!reference)
                            {
                                if (dWhat.Value.GetType() == typeof(List<dynamic>))
                                {
                                    var CastedCollection = InvokeCastCollection(Property, dWhat.Value);
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
                            else
                            {
                                var ReferenceObject = CastReference(pType, table, outerField, dWhat.Value, Property);
                                if (ReferenceObject != null)
                                {
                                    //ormCL.SpecialTypes : stringCL
                                    if (GetListType(Property.PropertyType) == typeof(Nullable) && GetListType(ReferenceObject.GetType()) != typeof(Nullable))
                                    {
                                        o.GetType()
                                            .GetProperty(Property.Name)
                                            .SetValue
                                            (o, ReferenceObject[0]);
                                    }
                                    else
                                    {
                                        ////Cast or it didn't happen
                                        //MethodInfo method = typeof(Enumerable).GetMethod("Cast");
                                        //MethodInfo generic = method.MakeGenericMethod(Property.PropertyType);
                                        //object[] parametersArray = new object[] { ReferenceObject };
                                        //object classInstance=new List<object>();
                                        //ReferenceObject = generic.Invoke(classInstance, parametersArray);
                                        o.GetType()
                                                .GetProperty(Property.Name)
                                                .SetValue
                                                (o, ReferenceObject);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return o;
        }

        protected object CastReference(Type pType, string table, string outerField, dynamic dWhatValue, PropertyInfo Property)
        {
            Type IsList = GetListType(pType);
            if(IsList!=typeof(Nullable))
            {
                pType = IsList;
            }           

            MethodInfo method = typeof(castedCL<T>).GetMethod("CastCollection");
            MethodInfo generic = method.MakeGenericMethod(pType);
            object classInstance = new castedCL<T>();
            baseCL b = new baseCL(dbPath);
            (classInstance as castedCL<T>).dbPath = this.dbPath;
            object[] parametersArray = new object[] { b.Select(new requestCL() { Table = new tableCl(table) }).Response.Response };
                       
            if(pType==typeof(string))
            {
                generic = method.MakeGenericMethod(typeof(stringCL));               
            }

            var ReferenceObject = generic.Invoke(classInstance, parametersArray);

            method = typeof(castedCL<T>).GetMethod("FilterIt");
            generic = method.MakeGenericMethod(pType);

            if (pType == typeof(string))
            {
                generic = method.MakeGenericMethod(typeof(stringCL));
            }

            Type IsCollection = GetListType(dWhatValue.GetType());
            if (IsCollection != typeof(Nullable))
            {
                MethodInfo method_inner = typeof(baseCL).GetMethod("CreateListOfM");
                MethodInfo generic_inner = method_inner.MakeGenericMethod(pType);
                object classInstance_inner = new baseCL("");
                object[] parametersArray_inner = new object[] { };
                var SomeCollection = generic_inner.Invoke(classInstance_inner, parametersArray_inner);
                foreach (dynamic Item in (dWhatValue as IList))
                {
                    parametersArray = new object[] { ReferenceObject, new conditionCL(outerField + ".==." + Item.AbsorbedValue) };
                    (SomeCollection as IList).Add((generic.Invoke(classInstance, parametersArray) as IList)[0]);  //Potential error. Please care about this.
                }
                ReferenceObject = SomeCollection;
            }
            else
            {
                parametersArray = new object[] { ReferenceObject, new conditionCL(outerField + ".==." + dWhatValue) };
                ReferenceObject = generic.Invoke(classInstance, parametersArray);
            }

            if ((ReferenceObject as ICollection).Count > 0)
            {
                if (pType == typeof(string))
                {
                    List<string> S = new List<string>();
                    foreach(stringCL ReferenceObjecItem in (List<stringCL>)ReferenceObject)
                    {
                        S.Add(ReferenceObjecItem.AbsorbedValue);
                    }
                    return S;
                }
                return ReferenceObject;
            }
            else
            {
                return null;
            }
        }
        protected void CheckAttributes(PropertyInfo Property, ref string name, ref bool absorbed, ref bool reference, ref string table, ref string outerField)
        {
            object[] attributes = Property.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType() == typeof(nameCLAttribute))
                {
                    name = (attributes[i] as nameCLAttribute).Name;
                }
                if (attributes[i].GetType() == typeof(absorbedCLAttribute))
                {
                    absorbed = true;
                }
                if (attributes[i].GetType() == typeof(referenceCLAttribute))
                {
                    reference = true;
                    table = (attributes[i] as referenceCLAttribute).Table;
                }
                if (attributes[i].GetType() == typeof(outerCLAttribute))
                {
                    outerField = (attributes[i] as outerCLAttribute).Key;
                }
            }
        }
        protected object InvokeCastCollection(PropertyInfo Property, dynamic dWhatValue)
        {
            MethodInfo method = typeof(castedCL<T>).GetMethod("CastCollection");
            var CollectionType = GetListType(Property.PropertyType);
            MethodInfo generic = method.MakeGenericMethod(CollectionType);
            object classInstance = new castedCL<T>();
            object[] parametersArray = new object[] { dWhatValue };
            return generic.Invoke(classInstance, parametersArray);
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
            Result = this.FilterIt<T>(Result, Conditions);
            return this;
        }
        public List<Y> FilterIt<Y>(List<Y> Collection, conditionCL Conditions)
        {
            List<Y> Filtered = new List<Y>();
            foreach (Y Row in Collection)
            {
                bool Add = false;
                foreach (string Condition in Conditions.Conditions)
                {
                    Add = CL.Solve(Row, Condition);
                }
                if (Add)
                    Filtered.Add(Row);
            }
            return Filtered;
        }
    }
}