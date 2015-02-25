using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class HealthState
    {
        [attributeCL]
        public int Id { get; set; }
        [attributeCL]
        public int HeroId { get; set; }
        public int CurrentHitPoints { get; set; }
        public int MaximumHitPoints { get; set; }
        public int AdditionalHitPoints { get; set; }
        public int Regeneration { get; set; }
        [referenceCL("/Hero/Common/Desease")]
        [outerCL("Id")]
        public string Desease { get; set; }
        [referenceCL("/Hero/Common/Intoxication")]
        [outerCL("Id")]
        public string Intoxication { get; set; }
        [referenceCL("/Hero/Common/Charm")]
        [outerCL("Id")]
        public string Charm { get; set; }
    }
}