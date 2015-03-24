using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.IO;
using System.Web;
using System.Xml.Linq;
using System.Collections;

namespace RuneFramework
{
    public abstract class Rune : IDisposable
    {
        public static RuneElement Element;

        private object loject;

        public Rune()
        {
            DataDirectory();
            SayRuneWords();
 
            //Console.WriteLine("Init end");
        }

        public void SaveRune()
        {
            foreach (PropertyInfo RuneWord in this.GetType().GetProperties())
            {
                RuneWord.GetValue(this, null).GetType().GetMethod("WriteRuneWord").Invoke(RuneWord.GetValue(this, null), new object[0]);
            }
        }

        protected void SayRuneWords()
        {
            foreach (PropertyInfo RuneWord in this.GetType().GetProperties())
            {
                string Name = RuneWord.PropertyType.GetGenericArguments()[0].Name;
                if (Name == "RuneString")
                    Name = RuneWord.Name;

                if (!Initialize(Name))
                    if (!CreateTable(Name))
                        throw new Exception("Can't create table");

                var Constructors = RuneWord.PropertyType.GetConstructors();
                foreach(var Constructor in Constructors)
                {
                    RuneWord.SetValue(this, Constructor.Invoke(new object[] { Name,this }));
                }
            }
        }

        protected void DataDirectory()
        {
            if (Element == RuneElement.Air)
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Data/")))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Data/"));
            }
            else if (Element == RuneElement.Earth)
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Data/"))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Data/");
        }

        protected bool Initialize(String Name)
        {
            if (Element == RuneElement.Air)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/Data/" + Name + ".xml")))
                    return true;
            }
            else if (Element == RuneElement.Earth)
                if (File.Exists(Directory.GetCurrentDirectory() + "/Data/" + Name + ".xml"))
                    return true;

            return false;
        }

        protected bool CreateTable(String Name)
        {
            lock (this.loject)
            {
                XDocument XmlTable = new XDocument(new XElement(Name + "s"));
                if (Element == RuneElement.Air)
                {
                    XmlTable.Save(HttpContext.Current.Server.MapPath("~/Data/" + Name + ".xml"));
                    return true;
                }
                else if (Element == RuneElement.Earth)
                {
                    XmlTable.Save(Directory.GetCurrentDirectory() + "/Data/" + Name + ".xml");
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {

        }
    }
}