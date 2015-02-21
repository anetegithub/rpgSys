using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Dynamic;
using System.Xml.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;

namespace rpgSys
{
    public class baseCL
    {
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

        public static object Safe;
        public bool Test = false;
        private string Path;
        public baseCL(string Path)
        {
            this.Path = Path;
        }

        private string GetPath(string Table)
        {
            if (!Test)
                return HttpContext.Current.Server.MapPath("~/" + Path + "/" + Table);
            else
                return System.IO.Directory.GetCurrentDirectory().Replace(@"rpgSys.Tests\bin\Debug", @"rpgSys\" + Path + @"\" + Table.Replace("/", @"\"));
        }

        public resultCL Select(requestCL Request)
        {
            List<Stat> info = new List<Stat>();
            XDocument doc = XDocument.Load(GetPath(Request.Table.Path));
            List<dynamic> Objects = new List<dynamic>();
            foreach (XElement el in doc.Root.Elements())
            {
                Objects.Add(DynamicElement(el));
            }
            return resultCL.ConnectResponse(new responseCL() { Response = Objects, Conditions = Request.Conditions != null ? Request.Conditions : null });
        }

        public returnCL Insert<T>(irequestCl<T> Request)
        {
            returnCL result = new returnCL("");
            try
            {
                XDocument doc = XDocument.Load(GetPath(Request.Table.Path));
                XElement Element = new XElement(typeof(T).Name);
                foreach (var Property in typeof(T).GetProperties())
                {
                    Element.Add(DynamicObject<T>(Request.Object, Property));
                }
                lock (Safe)
                {
                    doc.Root.Add(Element);
                    doc.Save(GetPath(Request.Table.Path));
                }
            }
            catch (Exception ex) { result = new returnCL(ex.Message) { Successful = false }; }
            result.Successful = true;

            return result;
        }

        private XElement DynamicObject<U>(U Object, PropertyInfo Property)
        {
            XElement Element = new XElement(Property.Name);
            Element.Value = Property.GetValue(Object, null).ToString();
            if (Property.PropertyType == typeof(List<>))
            {
                Type PropertyType = GetListType(Property.PropertyType);
                Type GenericListType = typeof(List<>).MakeGenericType(PropertyType);
                var List = (IList)Activator.CreateInstance(GenericListType);

                foreach (var Inner in List)
                {
                    foreach (var InnerProperty in Inner.GetType().GetProperties())
                    {
                        MethodInfo method = typeof(baseCL).GetMethod("DynamicObject");
                        var CollectionType = GetListType(Property.PropertyType);
                        MethodInfo generic = method.MakeGenericMethod(CollectionType);
                        object classInstance = new baseCL("");
                        object[] parametersArray = new object[] { Inner, InnerProperty };
                        var DeCastedProperty = generic.Invoke(classInstance, parametersArray);
                        Element.Add((XElement)DeCastedProperty);
                    }
                }
            }
            return Element;
        }

        private dynamic DynamicElement(XElement Element)
        {
            dynamic Object = new ExpandoObject();

            foreach (XAttribute a in Element.Attributes())
            {
                (Object as IDictionary<string, object>).Add(a.Name.LocalName, a.Value);
            }
            if (Element.Elements().Count() > 0)
            {
                List<dynamic> List = new List<dynamic>();

                foreach (XElement e in Element.Elements())
                {
                    List.Add(DynamicElement(e));
                }
                (Object as IDictionary<string, object>).Add(Element.Name.LocalName, List);
            }
            else
            {
                (Object as IDictionary<string, object>).Add(Element.Name.LocalName, Element.Value);
            }

            return Object;
        }
    }

    public class returnCL
    {
        public bool Successful = false;
        private string innermessage;
        public string InnerMessage
        {
            get
            {
                return innermessage;
            }
        }
        public returnCL(string InnerMessage)
        {
            innermessage = InnerMessage;
        }
    }

    public class requestCL
    {
        public tableCl Table;
        public conditionCL Conditions;
    }

    public class irequestCl<T>: requestCL
    {
        public T Object;
    }

    public class responseCL
    {
        public List<dynamic> Response;
        public conditionCL Conditions;
    }

    public class castedCL<T>
    {
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
            Result=Result.Take(Count).ToList<T>();
            return this;
        }
        public castedCL<T> Filter()
        {
            this.Filter(response.Conditions);
            return this;
        }
        public castedCL<T>Filter(conditionCL Conditions)
        {
            List<T> Filtered = new List<T>();
            foreach (T Row in Result)
            {
                bool Add=false;
                foreach(string Condition in Conditions.Conditions)
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

    public class sortingCL
    {
        public List<tokenCL> Sortings = new List<tokenCL>();
        public sortingCL(string Sortings)
        {
            foreach (string Sorting in Sortings.Split(','))
            {
                try
                {
                    this.Sortings.Add(
                        new tokenCL()
                        {
                            Field = Sorting.Split(':')[0],
                            Operation = Sorting.Split(':')[1]
                        });
                }
                catch { }
            }
        }
    }

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

    public class tableCl
    {
        private List<string> paths = new List<string>();
        public tableCl(string TableName)
        {
            paths.Add(TableName);
        }
        /// <summary>
        /// Nodes adds at start, like: Core=File, Node=Sample, result=Sample\File
        /// </summary>
        public string Node
        {
            set
            {
                paths.Add(value);
            }
        }

        public string Path
        {
            get
            {
                string r = "";
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    r = paths[i] + "/" + r;
                }
                return r.Substring(0, r.Length - 1) + ".xml";
            }
        }
    }

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
            foreach (string s in Conditions.Split(','))
            {
                try
                {
                    Tokens.Add(
                        new tokenCL()
                        {
                            Field = s.Split('.')[0],
                            Operation = s.Split('.')[1],
                            Value = s.Split('.')[2],
                        });
                }
                catch { }
            }
        }
    }

    public struct tokenCL
    {
        public string Field, Operation, Value;
    }

    public static class exampleCL
    {
        /// <summary>
        /// test class
        /// </summary>
        public class exCl
        {
            public string Name;
            public int Age;
            public string AgeLikeAGirl()
            {
                return (Age - 5 > 18 ? Age - 5 : 18).ToString();
            }
        }

        public static string Example()
        {
            //for look
            exCl x = new exCl() { Name = "Petro", Age = 5 };

            //create new connection
            baseCL Base = new baseCL("Data");
            Base.Test = true;

            //write ur conditions
            conditionCL s = new conditionCL("Age != 1, Name != Peter");

            //set table, don't forget about folders before table
            tableCl t = new tableCl("Characters");
            t.Node = "Character";
            t.Node = "Hero";

            //pack it into request
            requestCL r = new requestCL();
            r.Conditions = s;
            r.Table = t;

            //get value
            var value = Base.Select(r);

            //cast
            var casted = value.Cast<exCl>();

            //use results!
            exCl y = casted.UniqueResult();

            //Acctualy, it can looks like this:
            y = Base.Select(new requestCL() { Table = new tableCl("/Games/Chats/1") }).Cast<exCl>().UniqueResult();

            //Or like this:
            y = Base.Select(new requestCL()
            {
                Table = new tableCl("/Games/Chats/1")
            })
            .Cast<exCl>()
            .UniqueResult();

            Console.WriteLine(rules);

            //see how object was casted
            return y.NameAge() + "\n My Age is: " + y.AgeLikeAGirl();
        }

        /// <summary>
        /// test extension method
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string NameAge(this exCl ex)
        {
            return ex.Name + ": " + ex.Age.ToString();
        }


        private static readonly string rules =
@"
Two types of using CL:
1. JustCL:
    <Field.Operation.Value>
        
2. baseCL:
    main syntax:
        <baseCL_Operation?Condition,Condition,Condition&Object{Field=Value,Field=Value}&Object{Field=Value,Field=Value}>
    a) Select objects
        syntax:
            <s?Id.!=.0,Name!=Petro Is>
                LikeSQL:
                    SELECT * FROM table WHERE Id<>0 AND Name<>'Petro Is'
    b) Insert object(s)
        syntax:
            <i?Message{Id=1,HeroId=1,Master=False,System=False,Text=I like to move it move it, i like to, move it!!!}>
                LikeSQL:
                    INSERT INTO table(Id,HeroId,Master,System,Text) VALUES(1,1,false,false,'...')
    c) Update
        syntax:
            <u?Id.==.1,HeroId.==.1&Message{Id=1,HeroId=1,Master=False,System=False,Text=No more moves!}>
                LikeSQL:
                    UPDATE table(Id,HeroId,Master,System,Text) SET VALUES(1,1,false,false,'No more moves!')
    d) Delete
        syntax:
            <d?Id.==.1>
                LikeSQL:
                    DELETE FROM table WHERE Id=1
";
    }
}