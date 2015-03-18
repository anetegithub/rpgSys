using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;
using System.Dynamic;

namespace RuneFramework
{
    public static class Tablet<T>
    {
        public static XElement ToRunic(dynamic ObjectAtRunic)
        {
            XElement Element = new XElement(typeof(T).Name);

            foreach (var Field in (ObjectAtRunic as IDictionary<string, object>))
            {
                Element.Add(new XElement(Field.Key, Field.Value));
            }

            return Element;
        }

        public static dynamic ToWord(XElement RunicObject)
        {
            dynamic Word = new ExpandoObject();

            foreach(XElement RunicWord in RunicObject.Elements())
            {
                (Word as IDictionary<string, object>).Add(RunicWord.Name.LocalName, RunicWord.Value.ToString());
            }

            return Word;
        }
    }
}
