using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using RuneFramework;

namespace rpgSys
{
    public class User
    {
        public int Id { get; set; }
        public int HeroId { get; set; }
        public int GameId { get; set; }

        public string Avatar { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Stamp { get; set; }

        public List<UserActivity> Activity { get; set; }
    }
}