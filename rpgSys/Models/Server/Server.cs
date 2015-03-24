using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Server
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string UpTime { get; set; }
        public string ServerMessage { get; set; }
        public string MessageOfTheDay { get; set; }

        public int Modules { get; set; }
        public int Scenarios { get; set; }
        public int Tickets { get; set; }
        public int Characters { get; set; }
    }
}