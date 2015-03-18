using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RuneFramework;

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
            hr.Somes.Add(new Some() { A = 10, B = 0.019 });

            foreach (Some s in hr.Somes)
            {
                Console.WriteLine(s.A);
            }


            hr.Somes[hr.Somes.Count()-5].A = 2;

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

            hr.Somes[hr.Somes.Count() - 3].B = 0.159;

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
        }

        public class HeroRuna : Rune
        {
            public RuneWord<Hero> Heroes { get; set; }
            public RuneWord<Some> Somes { get; set; }
            public RuneWord<Sex> Sexes { get; set; }
        }

        public class Hero
        {
            public int HeroId { get; set; }

            public string Name { get; set; }

            public List<Some> Fields { get; set; }

            public Sex MySex { get; set; }
        }

        public class Some
        {
            public int SomeId { get; set; }

            public int A { get; set; }
            public double B { get; set; }
        }

        public enum Sex
        {
            Male = 0, Female = 1
        }
    }
}
