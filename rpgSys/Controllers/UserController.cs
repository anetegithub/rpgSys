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
            try { return Ok(new baseCL("Data").Select(new requestCL() { Table = new tableCl("/User/Users") }).Cast<User>().ToList().Count); }
            catch
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(string name, string psw)
        {
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
            string name="0", scenario="0";

            try
            {

                baseCL b = new baseCL("Data");

                //Hero
                string HeroId = b.Select(new requestCL() { Table = new tableCl("/User/Users") }).Cast<User>().Filter(new conditionCL("Id.==." + value)).ToList()[0].HeroId.ToString();
                List<Hero> Heroes = b.Select(new requestCL() { Table = new tableCl("/Hero/Info") }).Cast<Hero>().Filter(new conditionCL("Id.==." + HeroId)).ToList();
                if (Heroes.Count > 0)
                    name = Heroes[0].Name;

                //Scenario
                var Game = b.Select(new requestCL() { Table = new tableCl("/Games/Game") }).Cast<Game>().Filter(new conditionCL("Id.==." + Heroes[0].GameId.ToString())).ToList()[0].ScriptId;
                var Scenario = b.Select(new requestCL() { Table = new tableCl("/Scenario/Scenario") }).Cast<Scenario>().Filter(new conditionCL("Id.==." + Game.ToString())).ToList()[0].Title;
                if (Scenario != "")
                    scenario = Scenario;

            }
            catch { }

            return new string[] { name, scenario };
        }
    }

    public class Game
    {
        public int ScriptId;
    }
}