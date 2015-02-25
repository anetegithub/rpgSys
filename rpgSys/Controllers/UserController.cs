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
            var users = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/User/Users") }).Cast<User>().Filter(new conditionCL("Login.==." + name + ",Password.==." + psw)).ToList();
            if(users.Count==0)
            {
                return NotFound();
            }
            else if (users.Count==1)
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
