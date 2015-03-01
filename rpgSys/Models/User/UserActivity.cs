using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class UserActivity
    {
        [attributeCL]
        public int Id { get; set; }
        [attributeCL]
        public int UserId { get; set; }
        [referenceCL("/User/ActivityType")]
        [outerCL("Id")]
        public string Action { get; set; }
        public string Text { get; set; }
        public DateTime Stamp { get; set; }
        public string StampToString { get; set; }
    }
}