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
            var list = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Server/Chat") }).Cast<GeneralMessage>().Sort(new sortingCL("Id:Desc")).Limit(30).ToList();
            list.Reverse();
            return Ok(list);
        }

        [ActionName("send")]
        public string Post([FromBody]string Message)
        {
            string userName = Message.Split('`')[0];
            string userAvatar = Message.Split('`')[1];
            string msgText = Message.Split('`')[2];
            baseCL b = new baseCL("Data");
            int Id = b.Select(new requestCL() { Table = new tableCl("/Server/Chat") }).Cast<GeneralMessage>().ToList().Count + 1;
            return b.Insert<GeneralMessage>(new irequestCl()
            {
                Table = new tableCl("/Server/Chat"),
                Object = new GeneralMessage() { Id = Id, UserName = userName, UserAvatar = userAvatar, Stamp = DateTime.Now, Text = msgText }
            }).Successful.ToString();
        }
    }
}