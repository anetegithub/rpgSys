using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public class HealthState
    {
        public int Id { get; set; }

        public int CurrentHitPoints { get; set; }
        public int MaximumHitPoints { get; set; }
        public int AdditionalHitPoints { get; set; }
        public int Regeneration { get; set; }

        public RuneString Desease { get; set; }
        public RuneString Intoxication { get; set; }
        public RuneString Charm { get; set; }
    }
}