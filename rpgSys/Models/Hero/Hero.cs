using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public class Hero
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }

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
    }
}