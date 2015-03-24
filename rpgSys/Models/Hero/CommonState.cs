using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class CommonState
    {
        public int Id { get; set; }

        public int Initiative { get; set; }
        public int InitiativeSize { get; set; }
        public int InitiativeWisdom { get; set; }
        public int InitiativeMagic { get; set; }
        public int Speed { get; set; }
        public int SpeedFit { get; set; }
    }
}