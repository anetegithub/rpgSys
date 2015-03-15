using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;
using System.Dynamic;

namespace RuneFramework
{
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
                foreach (var v in (value as IDictionary<string, object>))
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
}
