using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rpgSys
{
    public class Scenario
    {
        public string Title { get; set; }
        public string Recomendation { get; set; }
        public string Fable { get; set; }
        public Location[] Locations { get; set; }
        public Npc[] Npcs { get; set; }
        public Event[] Events { get; set; }
        public Item[] Rewards { get; set; }
    }
    
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Map { get; set; }        
    }

    public class Npc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string Specification { get; set; }
        public Stat[] Stats { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public static class Additional
    {
        public static object[] Add(this object[] arr, object Item)
        {
            Array.Resize<object>(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = Item;
            return arr;
        }

        public static int[] Add(this int[] arr, int Item)
        {
            Array.Resize<int>(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = Item;
            return arr;
        }
    }
}