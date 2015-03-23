﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;
using RuneFramework;

namespace rpgSys
{
    //public class User
    //{
    //    [attributeCL]
    //    public int Id { get; set; }
    //    [attributeCL]
    //    public int HeroId { get; set; }
    //    [attributeCL]
    //    public int GameId { get; set; }

    //    public string Avatar { get; set; }
    //    public string Login { get; set; }
    //    public string Password { get; set; }
    //    public string Email { get; set; }
    //    public string Auth { get; set; }

    //    [stringifyCL]
    //    public DateTime Stamp { get; set; }

    //    public string StampToString { get; set; }
    //}

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
        public UserActivity Activity { get; set; }
    }
}