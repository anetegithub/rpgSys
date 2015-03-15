using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Collections;

using System.Xml.Linq;
using System.Web;
using System.IO;

using System.Dynamic;

namespace RuneFramework
{
    public class Transmuter<T> : IEnumerable<T>
    {
        public Transmuter()
        {
            //Init properties
            foreach (PropertyInfo Property in typeof(T).GetProperties())
            {
                if (Property.PropertyType.IsPrimitive)
                    Primitives.Add(Property);
                else if (Property.PropertyType == typeof(String))
                    Strings.Add(Property);
                else if (Property.PropertyType.IsGenericType)
                    Classes.Add(Property);
                else if (Property.PropertyType.IsEnum)
                    Enums.Add(Property);
            }

            //Init document
            string PathToFile = "";
            if (Rune.Element == RuneElement.Air)
                PathToFile = HttpContext.Current.Server.MapPath("~/Data/" + typeof(T).Name + ".xml");
            else if (Rune.Element == RuneElement.Earth)
                PathToFile = Directory.GetCurrentDirectory() + "/Data/" + typeof(T).Name + ".xml";

            Document = XDocument.Load(PathToFile);
        }
        
        protected void TransmuteToFile()
        {
            foreach (XElement Element in Document.Root.Elements())
            {
                Tablet<T> Item = new Tablet<T>(Element);
                //Item.WriteLetters
            }
        }

        protected void TransmuteFromFile()
        {
            foreach (XElement Element in Document.Root.Elements())
            {
                T Object = (T)Activator.CreateInstance(typeof(T));

                Tablet<T> Item = new Tablet<T>(Element);
                var ObjectAtRunic = Item.SayLetters;

                AboutLetters();
                //TransmutePrimitives(Element, ref Object);

                Words.Add(Object);
            }
        }

        protected void AboutLetters()
        { }

        protected List<PropertyInfo> Primitives = new List<PropertyInfo>();
        protected void TransmutePrimitives(XElement Element, ref T Object)
        {
            foreach (PropertyInfo Property in Primitives)
            {
                new PrimitiveLetter<T>(Element, ref Object, Property);
            }
        }


        protected List<PropertyInfo> Strings = new List<PropertyInfo>();
        protected List<PropertyInfo> Classes = new List<PropertyInfo>();
        protected List<PropertyInfo> Enums = new List<PropertyInfo>();

        protected XDocument Document;
        protected List<T> Words = new List<T>();
        protected List<dynamic> WordsOnRunic = new List<dynamic>();

        #region Interface
        
        public List<T> Get
        { get { return Words; } }

        public void Set(T Value, int Index)
        { Words[Index] = Value; }

        public void Add(T Item)
        {
            Words.Add(Item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T t in Words)
            {
                if (t == null)
                {
                    break;
                }
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }

    public class Tablet<T>
    {
        public Tablet(XElement Element)
        { this.Element = Element; }

        protected dynamic Word;
        protected XElement Element;

        public dynamic SayLetters
        {
            get
            {
                Word = new ExpandoObject();

                //Object-class
                if (Element.Elements().Count() > 0)
                    foreach (XElement Property in Element.Elements())
                        (Word as IDictionary<string, object>).Add(Property.Name.LocalName, Property.Value);
                //Enum
                else
                    (Word as IDictionary<string, object>).Add(Element.Name.LocalName, Element.Value);

                return Word;
            }
        }

        protected XElement _WritedLetters;
        public dynamic WriteLetters
        {
            set
            {
                _WritedLetters = new XElement(typeof(T).Name);
                foreach(var v in (value as IDictionary<string,object>))
                {
                    if (typeof(T).GetType().GetProperty(v.Key) != null)
                        _WritedLetters.Add(new XElement(v.Key, v.Value));
                }
            }
        }
        public XElement WritedLetters
        {
            get
            {
                return _WritedLetters;
            }
        }
    }


    public abstract class Letter<T>
    {
        public XElement Element;
        public T Object;
        public PropertyInfo Property;
        public void SetProperty()
        {
            foreach (XElement E in Element.Elements())
            {
                if (E.Name.LocalName == Property.Name)
                {
                    Property.SetValue(Object, Convert.ChangeType(E.Value, Property.PropertyType));
                }
            }
        }
    }

    public class PrimitiveLetter<T> : Letter<T>
    {
        public PrimitiveLetter(XElement Element, ref T Object, PropertyInfo Letter)
        {
            this.Element = Element;
            this.Object = Object;
            this.Property = Letter;
            this.SetProperty();
        }
    }


}