using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RuneFramework;

using System.Reflection.Emit;
using System.Reflection;

using System.Dynamic;

namespace RuneTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Rune.Element = RuneElement.Earth;

            Some s = new Some();
            s.Sex = 2;

            foreach (var Property in typeof(Some).GetProperties())
            {
                if (Property.PropertyType == typeof(RuneString))
                {
                    Console.WriteLine(Property.GetValue(s,null) as RuneString);
                }
            }
            
            using (var hr = new HeroRuna())
            {
                hr.Sex.Add(new RuneString("Male"));
                hr.Sex.Add(new RuneString("Female"));
                Console.WriteLine(hr.Sex[1]);
                hr.Somes[0].Sex = 2;
                Console.WriteLine(hr.Somes[0].Sex);

                /*
                 * 
                 * Output :
                 * Female
                 * 
                */

                //TestPrimitives(hr);
            }
            Console.WriteLine("done");
            Console.ReadLine();
        }

        static void TestPrimitives(HeroRuna hr)
        {
            hr.Somes.Add(new Some() { A = 10, B = 0.019 });

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.A);
            }


            //hr.Somes[hr.Somes.Count()-5].A = 2;
            //hr.Somes[hr.Somes.Count() - 5].Name = "NewName";
            //hr.Somes[hr.Somes.Count() - 5].Sexes = Sex.Female;

            hr.SaveRune();
            Console.WriteLine();
            Console.WriteLine("After Change Value And Save - ACVAS xD");

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.A);
            }

            Console.WriteLine();
            Console.WriteLine("B Values:");

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.B);
            }

            //hr.Somes[hr.Somes.Count() - 3].B = 0.159;

            hr.SaveRune();
            Console.WriteLine();
            Console.WriteLine("After Change B Value And Save - ACVAS xD");

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.B);
            }

            hr.Somes.Remove(0);

            Console.WriteLine();
            Console.WriteLine("After DELETE Value And Save - ACVAS xD");

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.B);
            }

            Console.WriteLine();
            Console.WriteLine("Strings: ");

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.Name ?? "");
            }
        }

        public class HeroRuna : Rune
        {
            public RuneWord<Hero> Heroes { get; set; }
            public RuneWord<Some> Somes { get; set; }
            public RuneWord<RuneString> Sex { get; set; }
        }
        public class Hero
        {
            public int HeroId { get; set; }

            public string Name { get; set; }

            public List<Some> Fields { get; set; }            
        }

        public class Some
        {
            public int SomeId { get; set; }

            public int A { get; set; }
            public double B { get; set; }

            public string Name { get; set; }

            public RuneString Sex { get; set; }
        }

        public enum Sex
        {
            Male = 0, Female = 1
        }
    }
}
