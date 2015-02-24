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
            baseCL b = new baseCL("Data");

            //new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Games/Chats/1") }).Cast<Message>().ToList();

            b.Test = false;
            Message m=new Message();
            m.Id=999;
            m.HeroId=999;
            m.Master=false;
            m.System=false;
            m.Text="Some text";

            Characteristics c=new Characteristics();
            c.Id=5;
            c.HeroId=4;
            c.Characteristic=new List<Characstic>()
            {
                new Characstic(){ DIX="dix", Name="1", Value="1"},
                new Characstic(){ DIX="dix", Name="2", Value="2"},
                new Characstic(){ DIX="dix", Name="3", Value="3"}
            };
            c.Attack = new AttackState()
            {
                HeroId = 4,
                Attack = 1,
                CritBonus = 0,
                CritChance = 0,
                Fit = 99,
                MaxDmg = 1,
                MinDmg = 2,
                WeaponMaxDmg = -1,
                WeaponMinDmg = -2
            };

            //var result = b.Insert<Characteristics>(new irequestCl<Characteristics>() { Table = new tableCl("/Hero/Character/Characteristics"), Object = c });

            //select
            var rsult = b.Select(new requestCL() { Table = new tableCl("/Hero/Character/Characteristics") }).Cast<Characteristics>().ToList();

           //insert
           // var sult = b.Insert<Characteristics>(new irequestCl() { Table = new tableCl("/Hero/Character/Characteristics"), Object = c });

            //var result = b.Insert<Message>(new irequestCl<Message>() { Table = new tableCl("/Games/Chats/1"), Object = m });
            /**/
            //var result = b.Select(new requestCL() { Table = new tableCl("/Games/Chats/1") }).Cast<Message>().Sort(new sortingCL("Id:Desc,HeroId:Desc")).Filter(new conditionCL("Id.==.1")).ToList();
            //var result = b.Request(new requestCl() { Statement = new statementCl(""), Table = new tableCl("/Hero/Character/Characteristics") }).Cast<Characteristics>();
            //var result = b.Select(new requestCl() { Statement = new conditionCL(""), Table = new tableCl("/Hero/Character/MaterialSkills") }).Cast<Skills>().Sort(new sortingCL("HeroId:Desc")).ToList();
            //foreach(Message m in result.ToList())
            //{
            //    if (m.Text != "f")
            //        m.Text = "hello world!";
            //}
            User tryFind = xmlBase.Users.GetByName(name, psw)[0];
            if (tryFind != null)
            {
                return Ok(tryFind);
            }
            else
            {
                return NotFound();
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
