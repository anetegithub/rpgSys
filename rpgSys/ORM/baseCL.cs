using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Dynamic;
using System.Xml.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace rpgSys
{
    public class baseCL
    {
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

        public resultCL Request(requestCl Request)
        {
            List<Stat> info = new List<Stat>();
            XDocument doc = XDocument.Load(GetPath(Request.Table.Path));
            List<dynamic> Objects = new List<dynamic>();
            foreach (XElement el in doc.Root.Elements())
            {
                Objects.Add(DynamicElement(el));
            }
            return resultCL.ConnectResponse(new responseCl() { Response = Objects });
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

    public class requestCl
    {
        public tableCl Table;
        public statementCl Statement;
    }

    public class responseCl
    {
        public List<dynamic> Response;
    }

    public class castedCl<T>
    {
        private U CreateObject<U>()
        {
            return (U)Activator.CreateInstance(typeof(U));
        }
        private List<T> Result = new List<T>();
        public castedCl(responseCl Response)
        {
            foreach (var DField in Response.Response)
            {
                var o = CreateObject<T>();
                foreach (var Property in typeof(T).GetProperties())
                {
                    foreach (var dWhat in DField)
                    {
                        if (Property.Name == dWhat.Key)
                        {
                            o.GetType()
                                .GetProperty(Property.Name)
                                .SetValue(
                                o,
                                Convert.ChangeType(dWhat.Value, Property.PropertyType));
                        }
                    }
                }
                Result.Add(o);
            }
        }

        public List<T> ToList()
        {
            return Result;
        }
        public T UniqueResult()
        {
            return Result[0];
        }
    }

    public class resultCL
    {
        private resultCL(responseCl Response)
        {
            response = Response;
        }
        private responseCl response;
        public responseCl Response
        { get { return response; } }
        public castedCl<T> Cast<T>()
        {
            return new castedCl<T>(response);
        }

        public static resultCL ConnectResponse(responseCl Response)
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

    public class statementCl
    {
        private List<tokenCl> Tokens = new List<tokenCl>();
        public statementCl(string Conditions)
        {
            foreach (string s in Conditions.Split(';'))
            {
                try
                {
                    Tokens.Add(
                        new tokenCl()
                        {
                            Field = s.Split(' ')[0],
                            Operation = s.Split(' ')[1],
                            Value = s.Split(' ')[2],
                        });
                }
                catch { }
            }
        }
    }

    public struct tokenCl
    {
        public string Field, Operation, Value;
    }

    public static class exampleCl
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
            statementCl s = new statementCl("Age != 1, Name != Peter");

            //set table, don't forget about folders before table
            tableCl t = new tableCl("Characters");
            t.Node = "Character";
            t.Node = "Hero";

            //pack it into request
            requestCl r = new requestCl();
            r.Statement = s;
            r.Table = t;

            //get value
            var value = Base.Request(r);

            //cast
            var casted = value.Cast<exCl>();

            //use results!
            exCl y = casted.UniqueResult();

            //Acctualy, it can looks like this:
            y = Base.Request(new requestCl() { Statement = new statementCl(""), Table = new tableCl("/Games/Chats/1") }).Cast<exCl>().UniqueResult();

            //Or like this:
            y = Base.Request(new requestCl()
            {
                Statement = new statementCl(""),
                Table = new tableCl("/Games/Chats/1")
            })
            .Cast<exCl>()
            .UniqueResult();

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
    }
}