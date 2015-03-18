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
            if (Rune.Element == RuneElement.Air)
                PathToFile = HttpContext.Current.Server.MapPath("~/Data/" + typeof(T).Name + ".xml");
            else if (Rune.Element == RuneElement.Earth)
                PathToFile = Directory.GetCurrentDirectory() + "/Data/" + typeof(T).Name + ".xml";

            Magican = new Lazy<RuneMage<T>>(() => new RuneMage<T>(PathToFile));            
        }

        protected string PathToFile;
        protected string Id
        {
            get
            {
                if (!typeof(T).IsEnum)
                {
                    var Id = typeof(T).GetProperty("Id");
                    if (Id == null)
                        Id = typeof(T).GetProperty(typeof(T).Name + "Id");
                    if (Id == null)
                        throw new Exception(typeof(T).Name + " class : Id not found!");
                    return Id.Name;
                }
                else
                    return "Id";
            }
        }

        public void Transmute()
        {
            //but first see changes
            //TransmuteToTablet();
        }

        protected XElement TransmuteToTablet(T Item)
        {
            dynamic WordAtRunic = new ExpandoObject();

            using (var Letter = new PrimitiveLetter<T>())
            {
                foreach (var Property in Primitives)
                {
                    Letter.GetProperty(ref WordAtRunic, Item, Property);
                }
            }
            return Tablet<T>.ToRunic(WordAtRunic as ExpandoObject);
        }

        protected void TransmuteFromTablet()
        {
            //foreach (XElement Element in Document.Root.Elements())
            //{
            //    T Object = (T)Activator.CreateInstance(typeof(T));

            //    Tablet<T> Item = new Tablet<T>(Element);
            //    var ObjectAtRunic = Item.SayLetters;

            //    AboutLetters();
            //    //TransmutePrimitives(Element, ref Object);

            //    Words.Add(Object);
            //}
        }

        protected void AboutLetters()
        { }

        protected List<PropertyInfo> Primitives = new List<PropertyInfo>();
        protected void TransmutePrimitives(XElement Element, ref T Object)
        {
            foreach (PropertyInfo Property in Primitives)
            {
                //new PrimitiveLetter<T>(Element, ref Object, Property);
            }
        }
        protected List<PropertyInfo> Strings = new List<PropertyInfo>();
        protected List<PropertyInfo> Classes = new List<PropertyInfo>();
        protected List<PropertyInfo> Enums = new List<PropertyInfo>();

        protected Lazy<RuneMage<T>> Magican;
        private RuneMage<T> ShadowMage { get; set; }
        protected RuneMage<T> Mage
        {
            get
            {
                if (!Magican.IsValueCreated)
                    ShadowMage = Magican.Value;

                return ShadowMage;
            }
        }

        protected List<T> Words = new List<T>();
        protected List<dynamic> WordsOnRunic = new List<dynamic>();

        public List<T> Get
        { get { return Words; } }

        public void Set(T Value, int Index)
        { Words[Index] = Value; }

        public void Add(T Item)
        {
            var MaxId = Mage.SelectMax(typeof(T).Name, this.Id);
            Item.GetType().GetProperty(Id).SetValue(Item, Convert.ChangeType(++MaxId, Item.GetType().GetProperty(Id).PropertyType));

            

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
    }
}