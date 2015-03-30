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
                if (Field.Value.GetType() != typeof(RuneList))
                    Element.Add(new XElement(Field.Key, Field.Value));
                else
                {
                    RuneList RList=(Field.Value as RuneList);

                    XElement Value = new XElement(Field.Key);

                    foreach (var Item in RList.List)
                        Value.Add(new XElement(RList.TypeName, Item));

                    Element.Add(Value);
                }
            }

            return Element;
        }

        public static dynamic ToWord(XElement RunicObject)
        {
            dynamic Word = new ExpandoObject();

            foreach(XElement RunicWord in RunicObject.Elements())
            {
                if (RunicWord.Elements().Count() == 0)
                {
                    (Word as IDictionary<string, object>).Add(RunicWord.Name.LocalName, RunicWord.Value.ToString());
                }
                else
                {
                    (Word as IDictionary<string, object>).Add(RunicWord.Name.LocalName, Collection(RunicWord));
                }
            }

            return Word;
        }

        private static RuneList Collection(XElement InnerRunicObject)
        {
            RuneList RuneList = new RuneList() { List = new List<object>() };

            foreach (XElement InnerRunicWord in InnerRunicObject.Elements())
            {
                RuneList.TypeName = InnerRunicWord.Name.LocalName;

                if (InnerRunicWord.Elements().Count() == 0)
                {
                    RuneList.List.Add(InnerRunicWord.Value.ToString());
                }
                else
                {
                    RuneList.List.Add(Collection(InnerRunicWord));
                }
            }

            return RuneList;
        }
                
    }
}
