using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public class Characteristic
    {
        public int Id { get; set; }

        public RuneString CharacteristicName { get; set; }
        public RuneString DIX { get; set; }

        public int Value { get; set; }
    }
}