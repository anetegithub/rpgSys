using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class ServerSettings
    {
        [attributeCL]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime UpTime { get; set; }
        public string ServerMessage { get; set; }
        public string MessageOfTheDay { get; set; }
        public int Modules { get; set; }
        public int Scenarios { get; set; }
        public int Tickets { get; set; }
        public int Characters { get; set; }
    }

    public class Ticket
    {

    }

    
        //[referenceCL("/Server/Modules")]
        //[outerCL("ServerId")]
        //public List<Module> Modules { get; set; }
        //[referenceCL("/Server/Scenarios")]
        //[outerCL("ServerId")]
        //public List<Scenario> Scenarios { get; set; }
        //[referenceCL("/Server/Tickets")]
        //[outerCL("ServerId")]
        //public List<Ticket> Tickets { get; set; }
        //[referenceCL("/Hero/Character/Info")]
        //[outerCL("ServerId")]
        //public List<Character> Info { get; set; }
}