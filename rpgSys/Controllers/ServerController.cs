using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

using ormCL;

namespace rpgSys.Controllers
{
    public class ServerController : ApiController
    {
        public IHttpActionResult Get()
        {
            ServerSettings Server = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Server/Settings") }).Cast<ServerSettings>().ToList()[0];
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
