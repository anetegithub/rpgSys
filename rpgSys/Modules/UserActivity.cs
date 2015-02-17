using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class UserActivity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<Activity> Activityes { get; set; }
    }

    public class Activity
    {
        public string Icon { get; set; }
        public string Info { get; set; }
        public string Stamp { get; set; }
    }
}