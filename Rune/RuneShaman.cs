using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

using System.Xml;

using System.IO;

using System.Collections;

namespace RuneFramework
{
    public sealed class RuneShaman<T>
    {
        public RuneShaman(String Path)
        {
            this.Path = Path;
        }
        private String Path;

        public XDocument Select(RuneBook Query)
        {
            return RuneTotem<T>.Totem.Select(Query, Path);
        }

        public XDocument Update(RuneBook Query)
        {
            return RuneTotem<T>.Totem.Update(Query, Path);
        }

        public XDocument Insert(RuneBook Query)
        {
            return RuneTotem<T>.Totem.Insert(Query, Path);
        }

        public XDocument Delete(RuneBook Query)
        {
            return RuneTotem<T>.Totem.Delete(Query, Path);
        }

        public Double LastId(String Field)
        {
            return RuneTotem<T>.Totem.LastId(Field, Path);
        }
    }
}