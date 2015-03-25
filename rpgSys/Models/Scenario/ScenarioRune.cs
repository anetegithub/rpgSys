using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public class ScenarioRune : Rune
    {
        public RuneWord<Scenario> Scenarios { get; set; }
        public RuneWord<Location> Locations { get; set; }
        public RuneWord<Event> Events { get; set; }
        public RuneWord<Npc> Npcs { get; set; }
        public RuneWord<NpcStat> NpcStats { get; set; }
        public RuneWord<Reward> Rewards { get; set; }
        public RuneWord<RuneString> Rare { get; set; }
        public RuneWord<RuneString> Target { get; set; }
        public RuneWord<RewardStat> RewardStats { get; set; }
        public RuneWord<RuneString> RewardInfo { get; set; }
        public RuneWord<RuneString> Desease { get; set; }
        public RuneWord<RuneString> Intoxication { get; set; }
        public RuneWord<RuneString> Charm { get; set; }
    }
}