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
                hr.Somes[0].Sex = 1;
                hr.SaveRune();
                Console.WriteLine(hr.Somes[0].Sex);
            }

            Console.WriteLine("done");
            Console.ReadLine();
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
