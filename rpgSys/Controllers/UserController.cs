using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

using System.Dynamic;

using ormCL;

namespace rpgSys
{
    public class UsersController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(xmlBase.Users.Get().Count);
        }

        public IHttpActionResult Get(string name, string psw)
        {
            Scenario s = new Scenario();
            s.Id = 1;
            s.Title = "ttle";
            s.Recomendation = "Recomend";
            s.Fable = "good game";
            s.Locations = new List<Location>()
            {
                new Location(){ Id=1, Description="desc", Map="mp", Name="nm", Specification="spec"},
                new Location(){ Id=2, Description="desc", Map="mp", Name="nm", Specification="spec"},
                new Location(){ Id=3, Description="desc", Map="mp", Name="nm", Specification="spec"}
            };
            s.Npcs = new List<Npc>()
            {
                new Npc(){ Id=1, Name="nme", Specification="spec", View="view", Stats=new NpcStat(){ Id=1, CRC=1}},
                new Npc(){ Id=2, Name="nme", Specification="spec", View="view"},
                new Npc(){ Id=3, Name="nme", Specification="spec", View="view", Stats=new NpcStat(){ Id=2, CON=5}}
            };
            s.Events = new List<Event>()
            {
                new Event(){ Id=1, Description="dsc", Title="ttle"},
                new Event(){ Id=2, Description="dsc", Title="ttle"},
                new Event(){ Id=3, Description="dsc", Title="ttle"},
            };
            //s.Rewards = new List<Item>()
            //{
            //    new Item(){ Id=1, Additional="add", Name="nme", Rare="rare", Who="wh"},
            //    new Item(){ Id=2, Additional="add", Name="nme", Rare="rare", Who="wh"},
            //    new Item(){ Id=3, Additional="add", Name="nme", Rare="rare", Who="wh"},
            //};
            s.Rewards = new List<Reward>()
            {
                new Reward(){ Id=1, Name="Меч истины", Rare="1", Target="2", Conditions="To be", Stats=new List<RewardStat>(){ 
                    new RewardStat(){ 
                        Id=1, RewardId=1, Value=5, 
                        Info=new RewardInfo(){ Id=5}
                    }
                }},
                new Reward(){ Id=2, Name="Меч истины", Rare="1", Target="2", Conditions="To be", Stats=new List<RewardStat>(){ 
                    new RewardStat(){ 
                        Id=2, RewardId=1, Value=5, 
                        Info=new RewardInfo(){ Id=10}
                    }
                }}
            };

            new baseCL("Data").Insert<Scenario>(new irequestCl() { Table = new tableCl("/Scenario/Scenario"), Object = s });




            var scenarios = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Scenario/Scenario") }).Cast<Scenario>().ToList();






            var users = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/User/Users") }).Cast<User>().Filter(new conditionCL("Login.==." + name + ",Password.==." + psw)).ToList();
            if (users.Count == 0)
            {
                return NotFound();
            }
            else if (users.Count == 1)
            {
                return Ok(users[0]);
            }
            else
            {
                return Conflict();
            }
        }

        [ActionName("profile")]
        public string[] Post([FromBody]string value)
        {
            var c = xmlBase.Characters.Profile(value);
            return new string[] { c.Name, c.Skin };
        }
    }
}