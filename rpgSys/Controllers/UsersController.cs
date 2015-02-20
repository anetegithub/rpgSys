using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

using System.Dynamic;

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
            b.Test = false;
            var result = b.Request(new requestCl() { Statement = new statementCl(""), Table = new tableCl("/Games/Chats/1") }).Cast<Message>();
            foreach(Message m in result.ToList())
            {
                if (m.Text != "f")
                    m.Text = "hello world!";
            }
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
