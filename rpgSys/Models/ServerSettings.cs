using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class ServerSettings
    {
        public string Name { get; set; }
        public DateTime UpTime { get; set; }
        public string ServerMessage { get; set; }
        public string MessageOfTheDay { get; set; }
        public int ServerModule { get; set; }
        public int ServerScenario { get; set; }
        public int ServerItem { get; set; }
        public int ServerCharacter { get; set; }
    }
}