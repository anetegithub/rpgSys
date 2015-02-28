using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Reward
    {
        [attributeCL]
        public int Id { get; set; }

        public string Name { get; set; }

        [referenceCL("/Reward/RewardRare")]
        [outerCL("Id")]
        public string Rare { get; set; }

        [referenceCL("/Reward/RewardTarget")]
        [outerCL("Id")]
        public string Target { get; set; }

        public string Conditions { get; set; }

        [referenceCL("/Reward/RewardStats")]
        [outerCL("RewardId")]
        public List<RewardStat> Stats { get; set; }
    }

    public class RewardStat
    {
        [attributeCL]
        public int Id { get; set; }

        [attributeCL]
        public int RewardId { get; set; }

        //[referenceCL("/Reward/RewardInfo")]
        //[outerCL("Id")]
        //public RewardInfo Info { get; set; }

        /* 
         * 
         * If you want use property like ENUM pls did it String.
         * If you did it another object result of this field will be all time insert/
         * 
         */
        [referenceCL("/Reward/RewardInfo")]
        [outerCL("Id")]
        public string Info { get; set; }

        public int Value { get; set; }
    }

    //public class RewardInfo
    //{
    //    [attributeCL]
    //    public int Id { get; set; }
    //    [absorbedCL]
    //    public string Name { get; set; }
    //}
}