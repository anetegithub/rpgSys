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
    public class ChatController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Server/Chat") }).Cast<GeneralMessage>().ToList());
        }

        [ActionName("send")]
        public string Put([FromBody]string Message)
        {
            GeneralMessage Msg = new JavaScriptSerializer().Deserialize<GeneralMessage>(Message);
            Msg.Id = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Server/Chat") }).Cast<GeneralMessage>().ToList().Count + 1;
            Msg.Stamp = DateTime.Now;
            return new baseCL("Data").Insert<GeneralMessage>(new irequestCl() { Table = new tableCl("/Server/Chat"), Object = Msg }).Successful.ToString();
        }
    }
}