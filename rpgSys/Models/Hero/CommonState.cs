using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public sealed class CommonState
    {
        public Int32 Id { get; set; }

        public Int32 Initiative { get; set; }
        public Int32 InitiativeSize { get; set; }
        public Int32 InitiativeWisdom { get; set; }
        public Int32 InitiativeMagic { get; set; }
        public Int32 Speed { get; set; }
        public Int32 SpeedFit { get; set; }

        public static CommonState operator +(CommonState A, CommonState B)
        {
            A.Initiative += B.Initiative;
            A.InitiativeSize += B.InitiativeSize;
            A.InitiativeWisdom += B.InitiativeWisdom;
            A.InitiativeMagic += B.InitiativeMagic;
            A.Speed += B.Speed;
            A.SpeedFit += B.SpeedFit;

            return A;
        }

        public static CommonState operator -(CommonState A, CommonState B)
        {
            A.Initiative -= B.Initiative;
            A.InitiativeSize -= B.InitiativeSize;
            A.InitiativeWisdom -= B.InitiativeWisdom;
            A.InitiativeMagic -= B.InitiativeMagic;
            A.Speed -= B.Speed;
            A.SpeedFit -= B.SpeedFit;

            return A;
        }
    }
}