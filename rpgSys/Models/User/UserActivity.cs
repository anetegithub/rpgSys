﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class UserActivity
    {
        public int Id { get; set; }

        public string Action { get; set; }
        public string Text { get; set; }
        public string Stamp { get; set; }
    }
}