using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections;

using RuneFramework;

namespace rpgSys
{
    public sealed class Hero
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Avatar { get; set; }

        public string Name { get; set; }
        public bool Active { get; set; }

        public int Level { get; set; }

        public int Expirience { get; set; }

        public RuneString Class { get; set; }
        public RuneString Race { get; set; }
        public RuneString Height { get; set; }
        public RuneString Sex { get; set; }

        public string God { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string Eyes { get; set; }
        public string Hair { get; set; }
        public string Skin { get; set; }

        public List<Characteristic> Characteristics { get; set; }
        public List<Ability> Abilities { get; set; }

        public HealthState HealthState { get; set; }

        public DefenceState DefenceState { get; set; }

        public AttackState AttackState { get; set; }

        public CommonState CommonState { get; set; }

        public List<Skill> Skills { get; set; }

        public RuneString Money { get; set; }
        public List<Stuff> Items { get; set; }

        public static Hero op_Add(Hero Hero, Stuff Item)
        {
            Hero.Class = Item.Class;
            Hero.Race = Item.Race;
            Hero.Height = Item.Height;
            Hero.Sex = Item.Sex;


            foreach (var Ch in Item.Characteristics)
                foreach (var Ch2 in Hero.Characteristics)
                    if (Ch.CharacteristicName == Ch2.CharacteristicName)
                        Hero.Characteristics[Hero.Characteristics.IndexOf(Ch2)] += Ch;

            foreach (var Ab in Item.Abilities)
                foreach (var Ab2 in Hero.Abilities)
                    if (Ab.AbilityName == Ab2.AbilityName)
                        Hero.Abilities[Hero.Abilities.IndexOf(Ab2)] += Ab;

            foreach (var Sk in Item.Skills)
                foreach (var Sk2 in Hero.Skills)
                    if (Sk.SkillName == Sk2.SkillName)
                        Hero.Skills[Hero.Skills.IndexOf(Sk2)] += Sk;

            Hero.HealthState += Item.HealthState;
            Hero.DefenceState += Item.DefenceState;
            Hero.AttackState += Item.AttackState;
            Hero.CommonState += Item.CommonState;


            return Hero;
        }

        public static Hero op_Substract(Hero Hero, Stuff Item)
        {
            foreach (var Ch in Item.Characteristics)
                foreach (var Ch2 in Hero.Characteristics)
                    if (Ch.CharacteristicName == Ch2.CharacteristicName)
                        Hero.Characteristics[Hero.Characteristics.IndexOf(Ch2)] -= Ch;

            foreach (var Ab in Item.Abilities)
                foreach (var Ab2 in Hero.Abilities)
                    if (Ab.AbilityName == Ab2.AbilityName)
                        Hero.Abilities[Hero.Abilities.IndexOf(Ab2)] -= Ab;

            foreach (var Sk in Item.Skills)
                foreach (var Sk2 in Hero.Skills)
                    if (Sk.SkillName == Sk2.SkillName)
                        Hero.Skills[Hero.Skills.IndexOf(Sk2)] -= Sk;

            Hero.HealthState -= Item.HealthState;
            Hero.DefenceState -= Item.DefenceState;
            Hero.AttackState -= Item.AttackState;
            Hero.CommonState -= Item.CommonState;

            return Hero;
        }

        public static Hero operator +(Hero Hero, Stuff Item)
        {
            return op_Add(Hero, Item);
        }

        public static Hero operator -(Hero Hero, Stuff Item)
        {
            return op_Substract(Hero, Item);
        }
    }
}