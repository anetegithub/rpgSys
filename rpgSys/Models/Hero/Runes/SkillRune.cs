using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public sealed class SkillRune : Rune
    {
        public RuneWord<Skill> Skills { get; set; }
        public RuneWord<RuneString> SkillName { get; set; }
        public RuneWord<SkillTemplate> SkillsTemplate { get; set; }
        public RuneWord<RuneString> DIX { get; set; }
    }
}