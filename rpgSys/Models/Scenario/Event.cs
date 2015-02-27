using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class Event
    {
        [attributeCL]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}