using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class GeneralChatMessage
    {
        public int Id { get; set; }        
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Stamp { get; set; }
        public string Text { get; set; }
    }
}