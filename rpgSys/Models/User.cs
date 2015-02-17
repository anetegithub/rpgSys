using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class User
    {
        public int Id { get; set; }
        public int HeroId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Auth { get; set; }
    }
}