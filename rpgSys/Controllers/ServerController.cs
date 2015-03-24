using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using RuneFramework;

namespace rpgSys.Controllers
{
    public class ServerController : ApiController
    {
        public IHttpActionResult Get()
        {
            using (var db = new Runes.ServerRune())
            {
                return Ok(db.Servers[0]);
            }
        }
    }
}