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

        public Rune()
        {
            DataDirectory();
            SayRuneWords();            
            Console.WriteLine("Init end");
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
                var T = RuneWord.PropertyType.GetGenericArguments()[0];

                if (!Initialize(T))
                    if (!CreateTable(T))
                        throw new Exception("Can't create table");

                var Constructors = RuneWord.PropertyType.GetConstructors();
                foreach(var Constructor in Constructors)
                {
                    RuneWord.SetValue(this, Constructor.Invoke(new object[0]));
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

        protected bool Initialize(Type Table)
        {
            if (Element == RuneElement.Air)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/Data/" + Table.Name + ".xml")))
                    return true;
            }
            else if (Element == RuneElement.Earth)
                if (File.Exists(Directory.GetCurrentDirectory() + "/Data/" + Table.Name + ".xml"))
                    return true;

            return false;
        }

        protected bool CreateTable(Type Table)
        {
            XDocument XmlTable = new XDocument(new XElement(Table.Name + "s"));
            if (Element == RuneElement.Air)
            {
                XmlTable.Save(HttpContext.Current.Server.MapPath("~/Data/" + Table.Name + ".xml"));
                return true;
            }
            else if (Element == RuneElement.Earth)
            {
                XmlTable.Save(Directory.GetCurrentDirectory() + "/Data/" + Table.Name + ".xml");
                return true;
            }

            return false;
        }

        public void Dispose()
        {

        }
    }

    //public class HeroRuna : Rune
    //{
    //    public RuneWord<Hero> Heroes { get; set; }
    //    public RuneWord<Some> Somes { get; set; }
    //    public RuneWord<Sex> Sexes { get; set; }
    //}

    //public class Hero
    //{
    //    public int HeroId { get; set; }

    //    public string Name { get; set; }

    //    public List<Some> Fields { get; set; }

    //    public Sex MySex { get; set; }
    //}

    //public class Some
    //{
    //    public int A { get; set; }
    //    public double B { get; set; }
    //}

    //public enum Sex
    //{
    //    Male = 0, Female = 1
    //}

}