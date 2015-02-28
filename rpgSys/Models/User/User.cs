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
        public string Login { get; set; }
        public string Password { get; set; }
        public string Auth { get; set; }
    }
}