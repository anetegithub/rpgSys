using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public class HeroRune : Rune
    {
        public RuneWord<Hero> Hero { get; set; }
        public RuneWord<RuneString> Class { get; set; }
        public RuneWord<RuneString> Race { get; set; }
        public RuneWord<RuneString> Height { get; set; }
        public RuneWord<RuneString> Sex { get; set; }
        public RuneWord<Characteristic> Characteristics { get; set; }
        public RuneWord<RuneString> CharacteristicName { get; set; }
        public RuneWord<RuneString> DIX { get; set; }
        public RuneWord<Ability> Abilities { get; set; }
        public RuneWord<RuneString> AbilityName { get; set; }
        public RuneWord<HealthState> HealthState { get; set; }
        public RuneWord<RuneString> Desease { get; set; }
        public RuneWord<RuneString> Intoxication { get; set; }
        public RuneWord<RuneString> Charm { get; set; }
        public RuneWord<DefenceState> DefenceState { get; set; }
        public RuneWord<AttackState> AttackState { get; set; }
        public RuneWord<CommonState> CommonState { get; set; }
        public RuneWord<Skill> Skills { get; set; }
        public RuneWord<RuneString> SkillName { get; set; }
    }

    public class HeroInfoRune : Rune
    {
        public RuneWord<Hero> Hero { get; set; }
        public RuneWord<RuneString> Class { get; set; }
        public RuneWord<RuneString> Race { get; set; }
        public RuneWord<RuneString> Height { get; set; }
        public RuneWord<RuneString> Sex { get; set; }
    }
}