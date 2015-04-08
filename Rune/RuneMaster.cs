using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

namespace RuneFramework
{
    public sealed class RuneMaster
    {
        public String Path;

        private XDocument _Document;
        public XDocument Document
        {
            get
            {
                if (_Document == null)
                    _Document = XDocument.Load(Path);
                return _Document;
            }
        }
        public void ReWrite(XElement InnerElements)
        {
            _Document = new XDocument(InnerElements);
            AddToLine(() =>
            {
                Document.Save(Path);
            });
        }

        public IEnumerable<XElement> Elements
        {
            get
            {
                return Document.Root.Elements();
            }
        }

        public void Close()
        {
            ReleaseLine();
        }

        private List<Action> Line = new List<Action>();

        public void AddToLine(Action Act)
        {
            Line.Add(Act);
        }

        public void ReleaseLine()
        {
            List<Action> NewLine = new List<Action>();
            foreach (Action Act in Line)
                Act();
            Line = NewLine;
        }
    }
}
