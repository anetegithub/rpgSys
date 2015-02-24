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
        /// <summary>
        /// for use this code IN your method just change List.M> to IList class
        /// </summary>
        /// <typeparam name="M">SHOULD be List.ComeType></typeparam>
        /// <returns></returns>
        public List<M> CreateListOfM_FromType_ListM<M>()
        {
            Type PropertyType = GetListType(typeof(M));
            Type GenericListType = typeof(List<>).MakeGenericType(PropertyType);
            return (List<M>)Activator.CreateInstance(GenericListType);
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

        public returnCL Insert<T>(irequestCl Request)
        {
            returnCL result = new returnCL("");
            result.Successful = true;
            try
            {
                XDocument doc = XDocument.Load(GetPath(Request.Table.Path));
                XElement Element = new XElement(typeof(T).Name);
                foreach (var Property in typeof(T).GetProperties())
                {
                    Element.Add(DynamicObject<T>((T)Request.Object, Property));
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
            string name = Property.Name, table = "", field = "";
            dynamicobjectType t = dynamicobjectType.Element;
            bool reference = false;


            atr(Property, ref t, ref name, ref reference, ref table, ref field);

            XObject Element = new XElement(name);

            if (!reference)
            {
                if (Property.GetValue(Object) is ICollection)
                {
                    foreach (var Inner in (Property.GetValue(Object) as ICollection))
                    {
                        XElement InnerElement = new XElement(Inner.GetType().Name);
                        foreach (var InnerProperty in Inner.GetType().GetProperties())
                        {
                            var DeCastedProperty = invoke_DynamicObject(Property, Inner, InnerProperty);

                            /* absorbed */
                            bool absorbed = false;
                            atr_inner(InnerProperty, DeCastedProperty, ref InnerElement, ref absorbed);
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
            }
            else
            {
                string value = "";
                foreach (var InnerProperty in Property.GetValue(Object, null).GetType().GetProperties())
                {
                    if (InnerProperty.Name == field)
                        value = InnerProperty.GetValue(Property.GetValue(Object, null), null).ToString();
                }
                (Element as XElement).Value = value;
               
               invoke_ReferenceWrite(Property.GetValue(Object,null),Property.PropertyType,table,field);
            }
            return Element;
        }
        protected void atr(PropertyInfo Property, ref dynamicobjectType t, ref String name, ref Boolean reference, ref String table, ref String field)
        {
            object[] attributes = Property.GetCustomAttributes(true);
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].GetType() == typeof(attributeCLAttribute))
                {
                    t = dynamicobjectType.Attribute;
                }
                if (attributes[i].GetType() == typeof(nameCLAttribute))
                {
                    name = (attributes[i] as nameCLAttribute).Name;
                }
                if (attributes[i].GetType() == typeof(referenceCLAttribute))
                {
                    reference = true;
                    table = (attributes[i] as referenceCLAttribute).Table;
                }
                if(attributes[i].GetType()==typeof(outerCLAttribute))
                {
                    field = (attributes[i] as outerCLAttribute).Key;
                }
            }
        }
        protected void atr_inner(PropertyInfo InnerProperty, Object DeCastedProperty, ref XElement InnerElement, ref Boolean absorbed)
        {
            var attributes = InnerProperty.GetCustomAttributes(true);
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
        }
        protected object invoke_DynamicObject(PropertyInfo Property, Object Inner, PropertyInfo InnerProperty)
        {
            MethodInfo method = typeof(baseCL).GetMethod("DynamicObject");
            var CollectionType = GetListType(Property.PropertyType);
            MethodInfo generic = method.MakeGenericMethod(CollectionType);
            object classInstance = new baseCL("");
            object[] parametersArray = new object[] { Inner, InnerProperty };
            return generic.Invoke(classInstance, parametersArray);
        }
        protected void invoke_ReferenceWrite(Object Object, Type pType,String table, String field)
        {
            MethodInfo method = typeof(baseCL).GetMethod("Insert");
            MethodInfo generic = method.MakeGenericMethod(pType);
            ParameterInfo[] parameters = generic.GetParameters();
            object classInstance = new baseCL(Path);
            object[] parametersArray = new object[] { new irequestCl() { Table = new tableCl(table), Object = Object } };
            var ReferenceObject = generic.Invoke(classInstance, parametersArray);

            if (!(ReferenceObject as returnCL).Successful)
            {
                Console.WriteLine((ReferenceObject as returnCL).InnerMessage);
            }
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

