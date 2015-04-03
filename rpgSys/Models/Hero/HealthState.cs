using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public sealed class HealthState
    {
        public Int32 Id { get; set; }

        public Int32 CurrentHitPoints { get; set; }
        public Int32 MaximumHitPoints { get; set; }
        public Int32 AdditionalHitPoints { get; set; }
        public Int32 Regeneration { get; set; }

        public RuneString Desease { get; set; }
        public RuneString Intoxication { get; set; }
        public RuneString Charm { get; set; }

        public static HealthState operator +(HealthState A, HealthState B)
        {
            A.CurrentHitPoints += B.CurrentHitPoints;
            A.MaximumHitPoints += B.MaximumHitPoints;
            A.AdditionalHitPoints += B.AdditionalHitPoints;
            A.Regeneration += B.Regeneration;
            A.Desease = B.Desease;
            A.Intoxication = B.Intoxication;
            A.Charm = B.Charm;

            return A;
        }

        public static HealthState operator -(HealthState A, HealthState B)
        {
            A.CurrentHitPoints -= B.CurrentHitPoints;
            A.MaximumHitPoints -= B.MaximumHitPoints;
            A.AdditionalHitPoints -= B.AdditionalHitPoints;
            A.Regeneration -= B.Regeneration;

            return A;
        }
    }
}