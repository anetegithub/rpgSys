using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ormCL;

namespace rpgSys.Controllers
{
    public class SkillsController : ApiController
    {
        public IHttpActionResult Get()
        {
            try { return Ok(new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Hero/Common/SkillList") }).Cast<Skill>().ToList()); }
            catch { return InternalServerError(); }
        }
    }
}