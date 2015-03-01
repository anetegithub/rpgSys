using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class User
    {
        [attributeCL]
        public int Id { get; set; }
        [attributeCL]
        public int HeroId { get; set; }
        [attributeCL]
        public int GameId { get; set; }

        public string Avatar { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Auth { get; set; }
        public DateTime Stamp { get; set; }
        public string StampToString { get; set; }
    }
}