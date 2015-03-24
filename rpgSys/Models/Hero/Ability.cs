using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RuneFramework;

namespace rpgSys
{
    public class Ability
    {
        public int Id { get; set; }

        public RuneString AbilityName { get; set; }

        public int Value { get; set; }
    }
}