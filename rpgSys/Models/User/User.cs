using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;
using RuneFramework;

namespace rpgSys
{
    public class User
    {
        public int Id { get; set; }

        public string Avatar { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Stamp { get; set; }

        public Hero Hero { get; set; }
        public Game Game { get; set; }
        public List<UserActivity> Activity { get; set; }
    }
}