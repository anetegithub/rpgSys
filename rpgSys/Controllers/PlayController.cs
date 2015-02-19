using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace rpgSys.Controllers
{
    public class PlayController : ApiControllerWithHub<PlayHub>
    {
        public string[] Get_Messages(string GameId, string Count, string Descending, string Select)
        {
            List<xmlBase.Where> conditions = new List<xmlBase.Where>();
            foreach (string condition in Select.Split(','))
            {
                string field = condition.Split(' ')[0];
                string _if = condition.Split(' ')[1];
                string value = condition.Split(' ')[2];               

                conditions.Add(new xmlBase.Where(
                    (Message m) =>
                    {
                        return ConditionLanguage.Compare<String>(_if, m.GetType().GetProperty(field).GetValue(m).ToString(), value);
                    })
               );
            }

            int gid, cnt;
            xmlBase.Where[] cod;
            bool dsc;

            if (!Int32.TryParse(GameId, out gid))
                return new string[0];

            if (!Int32.TryParse(Count, out cnt))
                return new string[0];

            if (!Boolean.TryParse(Descending, out dsc))
                return new string[0];

            cod = conditions.ToArray();

            return xmlBase.Chat.Get(gid, cnt, dsc, cod);
        }

        public string Put_Messages(string GameId,string HeroId,string IsMaster,string IsSystem,string Text)
        {
            int gid,hro;
            bool ism,iss;

            if (!Int32.TryParse(GameId, out gid))
                return "";

            if (!Int32.TryParse(HeroId, out hro))
                return "";

            if (!Boolean.TryParse(IsMaster, out ism))
                return "";

            if (!Boolean.TryParse(IsSystem, out iss))
                return "";

            return xmlBase.Chat.Set(gid, hro, ism, iss, Text).ToString();
        }

        //public Message Get_OneMessage(int id)
        //{
        //    lock (db)
        //    {
        //        var msg = db.SingleOrDefault(i => i.Id == id);
        //        if (msg == null)
        //            throw new HttpResponseException(
        //                Request.CreateResponse(HttpStatusCode.NotFound)
        //            );

        //        return msg;
        //    }
        //}

        //public HttpResponseMessage Post_AddMessage(Message item)
        //{
        //    lock (db)
        //    {
        //        // Add item to the "database"
        //        item.Id = Interlocked.Increment(ref lastId);
        //        db.Add(item);

        //        // Notify the connected clients
        //        Hub.Clients.All.addItem(item);

        //        // Return the new item, inside a 201 response
        //        var response = Request.CreateResponse(HttpStatusCode.Created, item);
        //        //string link = Url.Link("apiRoute", new { controller = "chat", id = item.Id });
        //        //response.Headers.Location = new Uri(link);
        //        return response;                
        //    }
        //}
    }
}
