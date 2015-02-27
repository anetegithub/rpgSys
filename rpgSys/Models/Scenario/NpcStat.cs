using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ormCL.Attributes;

namespace rpgSys
{
    public class NpcStat
    {
        [attributeCL]
        public int Id { get; set; }

        /// <summary>
        /// Constitution
        /// </summary>
        public int CON { get; set; }
        /// <summary>
        /// Fitness
        /// </summary>
        public int FIT { get; set; }
        /// <summary>
        /// Intelligence
        /// </summary>
        public int INT { get; set; }
        /// <summary>
        /// Wisdom
        /// </summary>
        public int WIS { get; set; }
        /// <summary>
        /// Charisma
        /// </summary>
        public int CHA { get; set; }
        /// <summary>
        /// Fate
        /// </summary>
        public int FTE { get; set; }

        /// <summary>
        /// Stamina
        /// </summary>
        public int STA { get; set; }
        /// <summary>
        /// Reflex
        /// </summary>
        public int RFX { get; set; }
        /// <summary>
        /// Will
        /// </summary>
        public int WIL { get; set; }

        /// <summary>
        /// Current hp
        /// </summary>
        public int CHP { get; set; }
        /// <summary>
        /// Maximum hp
        /// </summary>
        public int MHP { get; set; }
        /// <summary>
        /// Regeneration
        /// </summary>
        public int REG { get; set; }

        /// <summary>
        /// Desiase
        /// </summary>
        public string DES { get; set; }
        /// <summary>
        /// Intoxication
        /// </summary>
        public string INX { get; set; }
        /// <summary>
        /// Charm
        /// </summary>
        public string CHM { get; set; }

        /// <summary>
        /// Defence
        /// </summary>
        public int DEF { get; set; }
        /// <summary>
        /// Defence class
        /// </summary>
        public int DLS { get; set; }

        /// <summary>
        /// Attack
        /// </summary>
        public int ATK { get; set; }
        /// <summary>
        /// Minimal damage
        /// </summary>
        public int MID { get; set; }
        /// <summary>
        /// Maximal damage
        /// </summary>
        public int MAD { get; set; }

        /// <summary>
        /// Weapon minimal damage
        /// </summary>
        public int WID { get; set; }
        /// <summary>
        /// Weapon maximal damage
        /// </summary>
        public int WAD { get; set; }

        /// <summary>
        /// Crit chance
        /// </summary>
        public int CRC { get; set; }
        /// <summary>
        /// Crit bonus damage
        /// </summary>
        public int CRB { get; set; }

        /// <summary>
        /// Initiative
        /// </summary>
        public int INI { get; set; }
        /// <summary>
        /// Speed
        /// </summary>
        public int SPD { get; set; }
    }
}