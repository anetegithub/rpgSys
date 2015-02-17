using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Message
    {
        public int Id { get; set; }
        public bool Master { get; set; }
        public bool System { get; set; }
        public string Text { get; set; }
    }
}