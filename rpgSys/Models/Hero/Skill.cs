using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Skill
    {
        [attributeCL]
        public int Id { get; set; }

        [attributeCL]
        public int HeroId { get; set; }

        [referenceCL("Hero/Common/SkillInfo")]
        [outerCL("Id")]
        public SkillInfo Info { get; set; }

        public int Value { get; set; }
    }

    public class SkillInfo
    {
        [attributeCL]
        public int Id { get; set; }

        public string Name { get; set; }

        public string DIX { get; set; }
    }
}