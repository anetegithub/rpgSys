using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using RuneFramework;

namespace rpgSys.Controllers
{
    public class ChatController : ApiController
    {
        public IHttpActionResult Get()
        {           
            using(var db=new Runes.ServerRune())
            {
                return Ok(db.GeneralChat.Take(30));                
            }
        }

        [ActionName("send")]
        public string Post([FromBody]string Message)
        {
            string userName = Message.Split('`')[0];
            string userAvatar = Message.Split('`')[1];
            string msgText = Message.Split('`')[2];
            using(var db = new Runes.ServerRune())
            {
                GeneralChatMessage gcm = new GeneralChatMessage() { UserAvatar = userAvatar, UserName = userName, Text = msgText };
                gcm.Stamp = DateTime.Now.ToString();
                db.GeneralChat.Add(gcm);
                db.SaveRune();
                return "True";
            }
        }
    }
}