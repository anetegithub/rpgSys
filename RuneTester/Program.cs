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

            using (var hr = new HeroRuna())
            {
                TestPrimitives(hr);
            }

            Console.WriteLine("done");
            Console.ReadLine();
        }

        static void TestPrimitives(HeroRuna hr)
        {
            //hr.Somes.Add(new Some() { A = 10, B = 0.019 });

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.A);
            }


            hr.AdditionalClass.Add(new Somome() { C = 55 });

            Console.WriteLine(hr.Somes[0]);
            //hr.SaveRune();
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
            Console.WriteLine(hr.Somes[0].Sex);

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.Sex??"null");
            }

            hr.SaveRune();

            //hr.Somes.Remove(0);

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

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.Sex ?? "null");
            }
        }

        public class HeroRuna : Rune
        {
            public RuneWord<Hero> Heroes { get; set; }
            public RuneWord<Some> Somes { get; set; }
            public RuneWord<RuneString> Sex { get; set; }
            public RuneWord<RuneString> Height { get; set; }
            public RuneWord<Somome> AdditionalClass { get; set; }
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

            public Somome AdditionalClass { get; set; }
        }

        public class Somome
        {
            public int Id { get; set; }

            public int C { get; set; }
        }

        public enum Sex
        {
            Male = 0, Female = 1
        }
    }
}
