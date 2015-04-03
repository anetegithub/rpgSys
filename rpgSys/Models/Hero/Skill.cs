using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public sealed class Skill
    {
        public Int32 Id { get; set; }

        public RuneString SkillName { get; set; }
        public RuneString DIX { get; set; }

        public Int32 Value { get; set; }

        public static Skill operator +(Skill A, Skill B)
        {
            if (A.SkillName.Value == B.SkillName.Value)
                if (A.DIX.Value == B.DIX.Value)
                    A.Value += B.Value;

            return A;
        }

        public static Skill operator -(Skill A, Skill B)
        {
            if (A.SkillName.Value == B.SkillName.Value)
                if (A.DIX.Value == B.DIX.Value)
                    A.Value -= B.Value;

            return A;
        }
    }

    public sealed class SkillTemplate
    {
        public Int32 Id { get; set; }

        public RuneString SkillName { get; set; }
        public RuneString DIX { get; set; }
    }
}