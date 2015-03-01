using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class GeneralMessage
    {
        [attributeCL]
        public int Id { get; set; }        
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public DateTime Stamp { get; set; }
        public string Text { get; set; }
    }
}