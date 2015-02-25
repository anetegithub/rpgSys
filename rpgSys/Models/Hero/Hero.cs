using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Hero
    {
        [attributeCL]
        public int Id { get; set; }

        [attributeCL]
        public int UserId { get; set; }

        [attributeCL]
        public int GameId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int Expirience { get; set; }

        [referenceCL("/Hero/Common/Class")]
        [outerCL("Id")]
        public string Class { get; set; }

        [referenceCL("/Hero/Common/Race")]
        [outerCL("Id")]
        public string Race { get; set; }

        public string God { get; set; }

        [referenceCL("/Hero/Common/Height")]
        [outerCL("Id")]
        public string Height { get; set; }

        public int Age { get; set; }

        [referenceCL("/Hero/Common/Sex")]
        [outerCL("Id")]
        public string Sex { get; set; }

        public double Weight { get; set; }

        public string Eyes { get; set; }

        public string Hair { get; set; }

        public string Skin { get; set; }

        [referenceCL("/Hero/Characteristic")]
        [outerCL("HeroId")]
        public List<Characteristic> Characteristics { get; set; }

        [referenceCL("/Hero/Ability")]
        [outerCL("HeroId")]
        public List<Ability> Abilities { get; set; }

        [referenceCL("/Hero/HealthState")]
        [outerCL("HeroId")]
        public HealthState HealthState { get; set; }

        [referenceCL("/Hero/DefenceState")]
        [outerCL("HeroId")]
        public DefenceState DefenceState { get; set; }

        [referenceCL("/Hero/AttackState")]
        [outerCL("HeroId")]
        public AttackState AttackState { get; set; }

        [referenceCL("/Hero/CommonState")]
        [outerCL("HeroId")]
        public CommonState CommonState { get; set; }

        [referenceCL("/Hero/MaterialSkill")]
        [outerCL("HeroId")]
        public List<Skill> MaterialSkill { get; set; }

        [outerCL("HeroId")]
        [referenceCL("/Hero/MentalSkill")]
        public List<Skill> MentalSkill { get; set; }

        [outerCL("HeroId")]
        [referenceCL("/Hero/ClassSkill")]
        public List<Skill> ClassSkill { get; set; }
    }
}