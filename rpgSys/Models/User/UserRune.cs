using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys.Runes
{
    public class UserRune : Rune
    {
        public RuneWord<User> Users { get; set; }
        public RuneWord<UserActivity> Activity { get; set; }
    }
}