using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rpgSys.Controllers
{
    public class ServersController : ApiController
    {
        public IHttpActionResult Get()
        {
            var Server = xmlBase.Get();
            if(Server!=null)
            {
                return Ok(Server);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
