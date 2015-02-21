using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Skills
    {
        public int HeroId { get; set; }
        public List<Skill> SkillsList { get; set; }
    }

    public class Skill
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int Bonus { get; set; }
    }
}