using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public class Reward
    {      
        public int Id { get; set; }

        public string Name { get; set; }

        public RuneString Rare { get; set; }

        public RuneString Target { get; set; }

        public string Additional { get; set; }

        public string Conditions { get; set; }

        public List<RewardStat> RewardStats { get; set; }
    }

    public class RewardStat
    {
        public int Id { get; set; }

        public RuneString RewardInfo { get; set; }

        public int Value { get; set; }
    }
}