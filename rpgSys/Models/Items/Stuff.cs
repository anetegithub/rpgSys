using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public sealed class Stuff
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public Boolean IsOnHero { get; set; }

        public String Additional { get; set; }

        public List<Characteristic> Characteristics { get; set; }

        public List<Ability> Abilities { get; set; }

        public HealthState HealthState { get; set; }

        public DefenceState DefenceState { get; set; }

        public AttackState AttackState { get; set; }

        public CommonState CommonState { get; set; }

        public List<Skill> Skills { get; set; }

        public RuneString Class { get; set; }
        public RuneString Race { get; set; }
        public RuneString Height { get; set; }
        public RuneString Sex { get; set; }

        public void Dress(Hero Hero)
        {
            var Reference = (from b in Hero.Items where b.Id == this.Id select b).ToList();
            if (Reference.Count != 0)
            {
                this.IsOnHero = true;
                Hero += this;
            }
        }

        public void UnDress(Hero Hero)
        {
            var Reference = (from b in Hero.Items where b.Id == this.Id select b).ToList();
            if (Reference.Count != 0)
            {
                this.IsOnHero = false;
                Hero -= this;
            }
        }
    }

    internal sealed class StuffInstructions
    {
        public Int32 UserId { get; set; }
        public Int32 HeroId { get; set; }
        public Int32 ItemId { get; set; }
        public Boolean Dress { get; set; }
    }
}