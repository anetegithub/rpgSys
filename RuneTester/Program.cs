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
                //hr.Heroes.Add(new Hero());
                //var v = (from b in hr.Heroes where b.HeroId > 0 select b).ToList();                
                //connection.Heroes//[0].HeroId = 5;
                hr.Somes.Add(new Some() { A = 12, B = 0.009 });

                hr.SaveRune();
            }
            Console.WriteLine("done");
            Console.ReadLine();
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
