using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public class Skill
    {
        public int Id { get; set; }

        public RuneString SkillName { get; set; }
        public RuneString DIX { get; set; }

        public int Value { get; set; }
    }

    public class SkillTemplate
    {
        public int Id { get; set; }

        public RuneString SkillName { get; set; }
        public RuneString DIX { get; set; }
    }
}