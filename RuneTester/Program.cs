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

            FirstInit();

            SecondInit();

            ThirdInit();

            //Console.WriteLine("done");
            Console.ReadLine();
        }

        static void FirstInit()
        {
            using (var Session=new HeroRune())
            {
                Session.Sex.Add(new RuneString("Male"));
                Session.Sex.Add(new RuneString("Female"));

                Session.PersonalPet.Add(new Pet() { Name = "Felix" });

                Hero H = new Hero();
                H.Age = 21;
                H.Name = "Paul";
                H.Weght = 67.12;
                H.Young = true;

                H.Sex = 1;

                H.PersonalPet = Session.PersonalPet[0];

                Console.WriteLine("Paul pet name before save is " + H.PersonalPet.Name);

                Session.Hero.Add(H);
                Session.SaveRune();

                Console.WriteLine("Paul pet name after save is " + H.PersonalPet.Name);
            }
        }

        static void SecondInit()
        {
            using (var Session =new AdditionalHeroRune())
            {
                Session.PetsFood.Add(new Food() { FoodName = "Fish", FoodCount = 5 });
                Session.PetsFood.Add(new Food() { FoodName = "Meat", FoodCount = 5 });

                Session.PersonalPet[0].PetsFood = Session.PetsFood[0];

                /* Exception
                 *  Session.Hero[0].PersonalPet.PetsFood = Session.PetsFood[0];
                 */

                Session.SaveRune();

                /* Not exception but not saved yet */
                Session.Hero[0].PersonalPet.PetsFood = Session.PetsFood[1];

                Console.WriteLine();
                Console.WriteLine("Paul pet food before save is " + Session.Hero[0].PersonalPet.PetsFood.FoodName);
            }
            using (var Session = new AdditionalHeroRune())
            {
                Console.WriteLine("But Paul pet food actually is "+Session.Hero[0].PersonalPet.PetsFood.FoodName);
            }
        }

        static void ThirdInit()
        {
            using (var Session = new HeroRune())
            {
                Console.WriteLine();
                Console.WriteLine("So, in HeroRune we have this Paul: ");
                Hero h = Session.Hero[0];
                Console.WriteLine(h.Name);
                Console.WriteLine(h.Age);
                Console.WriteLine(h.Weght);
                Console.WriteLine(h.Young);
                Console.WriteLine(h.Sex);
                Console.WriteLine(h.PersonalPet.Name);
                if (h.PersonalPet.PetsFood != null)
                    Console.WriteLine(h.PersonalPet.PetsFood.FoodName);
                else
                    Console.WriteLine("Pet have NULL food");
            }
            using (var Session = new AdditionalHeroRune())
            {
                Console.WriteLine();
                Console.WriteLine("But in AdditionalHeroRune we have another Paul: ");
                Hero h = Session.Hero[0];
                Console.WriteLine(h.Name);
                Console.WriteLine(h.Age);
                Console.WriteLine(h.Weght);
                Console.WriteLine(h.Young);
                Console.WriteLine(h.Sex);
                Console.WriteLine(h.PersonalPet.Name);
                Console.WriteLine("Pet have " + h.PersonalPet.PetsFood.FoodName + " food");
            }
        }

        public class HeroRune : Rune
        {
            public RuneWord<Hero> Hero { get; set; }
            public RuneWord<RuneString> Sex { get; set; }
            public RuneWord<Pet> PersonalPet { get; set; }
        }

        public class AdditionalHeroRune : Rune
        {
            public RuneWord<Hero> Hero { get; set; }
            public RuneWord<RuneString> Sex { get; set; }
            public RuneWord<Pet> PersonalPet { get; set; }
            public RuneWord<Food> PetsFood { get; set; }
        }

        public class Hero
        {
            public int HeroId { get; set; }

            public string Name { get; set; }
            public int Age { get; set; }
            public double Weght { get; set; }
            public bool Young { get; set; }

            public RuneString Sex { get; set; }

            public Pet PersonalPet { get; set; }
        }

        public class Pet
        {
            public int Id { get; set; }

            public string Name { get; set; }
            public Food PetsFood { get; set; }
        }

        public class Food
        {
            public int FoodId { get; set; }

            public string FoodName { get; set; }
            public int FoodCount { get; set; }
        }
    }
}
