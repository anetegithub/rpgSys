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

using ormCL.Attributes;

namespace ormCL
{
    //wtf
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

        public static object Safe = new object();
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
            XDocument doc = XDocument.Load(GetPath(Request.Table.Path));
            List<dynamic> Objects = new List<dynamic>();
            foreach (XElement el in doc.Root.Elements())
            {
                Objects.Add(DynamicElement(el));
            }
            return resultCL.ConnectResponse(new responseCL() { Response = Objects, Conditions = Request.Conditions != null ? Request.Conditions : null, dbPath = Path });
        }

        public returnCL Insert<T>(irequestCl<T> Request)
        {
            returnCL result = new returnCL("");
            result.Successful = true;
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
            return result;
        }

        public XObject DynamicObject<U>(U Object, PropertyInfo Property)
        {
            string name = Property.Name;
            dynamicobjectType t = dynamicobjectType.Element;

            object[] attributes = Property.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType() == typeof(attributeCLAttribute))
                {
                    t = dynamicobjectType.Attribute;
                }
                else if (attributes[i].GetType() == typeof(nameCLAttribute))
                {
                    name = (attributes[i] as nameCLAttribute).Name;
                }
            }

            XObject Element = new XElement(name);

            if (Property.GetValue(Object) is ICollection)
            {
                Type PropertyType = GetListType(Property.PropertyType);
                Type GenericListType = typeof(List<>).MakeGenericType(PropertyType);
                var List = (IList)Activator.CreateInstance(GenericListType);

                foreach (var Inner in (Property.GetValue(Object) as ICollection))
                {
                    XElement InnerElement = new XElement(Inner.GetType().Name);
                    foreach (var InnerProperty in Inner.GetType().GetProperties())
                    {
                        MethodInfo method = typeof(baseCL).GetMethod("DynamicObject");
                        var CollectionType = GetListType(Property.PropertyType);
                        MethodInfo generic = method.MakeGenericMethod(CollectionType);
                        object classInstance = new baseCL("");
                        object[] parametersArray = new object[] { Inner, InnerProperty };
                        var DeCastedProperty = generic.Invoke(classInstance, parametersArray);

                        /* absorbed */
                        bool absorbed = false;
                        attributes = InnerProperty.GetCustomAttributes(true);
                        for (int i = 0; i < attributes.Length; i++)
                        {
                            if (attributes[i].GetType() == typeof(absorbedCLAttribute))
                            {
                                if (DeCastedProperty.GetType() == typeof(XElement))
                                    InnerElement.Value = (DeCastedProperty as XElement).Value;
                                if (DeCastedProperty.GetType() == typeof(XAttribute))
                                    InnerElement.Value = (DeCastedProperty as XAttribute).Value;

                                absorbed = true;
                            }
                        }
                        if (!absorbed)
                            InnerElement.Add((XObject)DeCastedProperty);
                    }
                    (Element as XElement).Add(InnerElement);
                }
            }
            else
            {
                if (t == dynamicobjectType.Attribute)
                    Element = new XAttribute(Property.Name, Property.GetValue(Object, null).ToString());

                if (t == dynamicobjectType.Element)
                    (Element as XElement).Value = Property.GetValue(Object, null).ToString();
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

            string Collection = IsCollection(Element);

            if (Collection != "")
            {
                List<dynamic> List = new List<dynamic>();
                foreach (XElement e in Element.Elements())
                {
                    List.Add(DynamicElement(e));
                }
                Object = List;
            }
            else
            {
                foreach (XElement e in Element.Elements())
                {
                    if (IsCollection(e) == "")
                        (Object as IDictionary<string, object>).Add(e.Name.LocalName, e.Value);
                    else
                        (Object as IDictionary<string, object>).Add(e.Name.LocalName, DynamicElement(e));
                }
                (Object as IDictionary<string, object>).Add("AbsorbedValue", Element.Value);
            }

            return Object;
        }

        private string IsCollection(XElement Element)
        {
            List<XElement> List = Element.Elements().ToList<XElement>();
            if (List.Count == 0)
                return "";
            if (List.Count == 1)
                return "";
            if (List.Count > 2)
            {
                if (List[0].Name.LocalName == List[1].Name.LocalName)
                    return List[0].Name.LocalName;
                else
                    return "";
            }
            return "";
        }
    }
}

