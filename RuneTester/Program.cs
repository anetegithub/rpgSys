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

            FirstExample();

            SecondExample();

            ThirdExample();

            FourExample();

            Console.ReadLine();
        }

        static void FirstExample()
        {
            using (var db = new HeroRune())
            {
                db.Sex.Add(new RuneString("Male"));
                db.Sex.Add(new RuneString("Female"));

                db.PersonalPet.Add(new Pet() { Name = "Felix" });

                Hero H = new Hero();
                H.Age = 21;
                H.Name = "Paul";
                H.Weght = 67.12;
                H.Young = true;

                H.Sex = 1;

                H.PersonalPet = db.PersonalPet[0];

                Console.WriteLine("Paul pet name before save is " + H.PersonalPet.Name);

                db.Hero.Add(H);
                db.SaveRune();

                Console.WriteLine("Paul pet name after save is " + H.PersonalPet.Name);
            }
        }

        static void SecondExample()
        {
            using (var db = new AdditionalHeroRune())
            {
                db.PetsFood.Add(new Food() { FoodName = "Fish", FoodCount = 5 });
                db.PetsFood.Add(new Food() { FoodName = "Meat", FoodCount = 5 });

                db.PersonalPet[0].PetsFood = db.PetsFood[0];

                /* Exception
                 *  Session.Hero[0].PersonalPet.PetsFood = Session.PetsFood[0];
                 */

                db.SaveRune();

                /* Not exception but not saved yet */
                db.Hero[0].PersonalPet.PetsFood = db.PetsFood[1];

                Console.WriteLine();
                Console.WriteLine("Paul pet food before save is " + db.Hero[0].PersonalPet.PetsFood.FoodName);
            }
            using (var Session = new AdditionalHeroRune())
            {
                Console.WriteLine("But Paul pet food actually is " + Session.Hero[0].PersonalPet.PetsFood.FoodName);
            }
        }

        static void ThirdExample()
        {
            using (var db = new HeroRune())
            {
                Console.WriteLine();
                Console.WriteLine("So, in HeroRune we have this Paul: ");
                Hero h = db.Hero[0];
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
            using (var db = new AdditionalHeroRune())
            {
                Console.WriteLine();
                Console.WriteLine("But in AdditionalHeroRune we have another Paul: ");
                Hero h = db.Hero[0];
                Console.WriteLine(h.Name);
                Console.WriteLine(h.Age);
                Console.WriteLine(h.Weght);
                Console.WriteLine(h.Young);
                Console.WriteLine(h.Sex);
                Console.WriteLine(h.PersonalPet.Name);
                Console.WriteLine("Pet have " + h.PersonalPet.PetsFood.FoodName + " food");
            }
        }

        static void FourExample()
        {
            using(var db = new FullHeroRune())
            {
                Console.WriteLine();
                Console.WriteLine("Let's go create Paul's cloth: ");
                Console.WriteLine();

                Console.WriteLine("1 Step : create threads by 3, 5, 6 positions");
                db.MainThreads.Add(new Threads() { Positions = new List<int>() { 3, 5, 6 } });

                Console.WriteLine();
                Console.WriteLine("2 Step : create cloth types (enums) like T-shirt and Pants");
                db.ClothTYPE.Add("T-shirt");
                db.ClothTYPE.Add("Pants");

                Console.WriteLine();
                Console.WriteLine("3 Step : create two Cloth Pants type from threads(3,5,6)");
                for(int i=0;i<2;i++)
                {
                    db.HeroClothes.Add(new Cloth() { ClothTYPE = 2, Size = 34, MainThreads = db.MainThreads[0] });
                }
                Console.WriteLine();
                Console.WriteLine("4 Step : set Paul new cloth");
                db.Hero[0].HeroClothes = new List<Cloth>() { db.HeroClothes[0], db.HeroClothes[1] };

                Console.WriteLine();
                Console.WriteLine("5 Step : let's see what we have done!");
                foreach (var Cloth in db.Hero[0].HeroClothes)
                {
                    Console.WriteLine("Paul have cloth:");
                    Console.WriteLine();
                    Console.WriteLine("Cloth type is " + Cloth.ClothTYPE);
                    Console.WriteLine("Cloth size is " + Cloth.Size);
                    Console.Write("Threads positions is ");
                    foreach (var Position in Cloth.MainThreads.Positions)
                    {
                        Console.Write(Position + " ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("6 Step : Ok! Now we can save it...");
                db.SaveRune();

                Console.WriteLine();
                Console.WriteLine("7 Step : and see again AFTER save");
                Console.WriteLine();
                foreach (var Cloth in db.Hero[0].HeroClothes)
                {
                    Console.WriteLine("Paul have cloth:");
                    Console.WriteLine();
                    Console.WriteLine("Cloth type is " + Cloth.ClothTYPE);
                    Console.WriteLine("Cloth size is " + Cloth.Size);
                    Console.Write("Threads positions is ");
                    foreach (var Position in Cloth.MainThreads.Positions)
                    {
                        Console.Write(Position + " ");
                    }
                    Console.WriteLine();
                }

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

        public class FullHeroRune : Rune
        {
            public RuneWord<Hero> Hero { get; set; }
            public RuneWord<RuneString> Sex { get; set; }
            public RuneWord<Pet> PersonalPet { get; set; }
            public RuneWord<Food> PetsFood { get; set; }
            public RuneWord<RuneString> ClothTYPE { get; set; }
            public RuneWord<Cloth> HeroClothes { get; set; }
            public RuneWord<Threads> MainThreads { get; set; }
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

            public List<string> Coords { get; set; }
            public List<Cloth> HeroClothes { get; set; }
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

        public class Cloth
        {
            public int Id { get; set; }

            public RuneString ClothTYPE { get; set; }

            public int Size { get; set; }

            public Threads MainThreads { get; set; }
        }

        public class Threads
        {
            public int ThreadsId { get; set; }

            public List<int> Positions { get; set; }
        }
    }
}