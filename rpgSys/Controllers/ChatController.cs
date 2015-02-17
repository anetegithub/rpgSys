using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Threading;
using SignalR;
using SignalR.Hubs;

namespace rpgSys.Controllers
{
    public class ChatController : ApiControllerWithHub<MessageHub>
    {
        private static List<Message> db = xmlBase.Chat.Data;
        private static int lastId = db.Max(tdi => tdi.Id);

        public IEnumerable<Message> Get_AllMessages()
        {
            lock (db)
                return db.ToArray();
        }

        public Message Get_OneMessage(int id)
        {
            lock (db)
            {
                var msg = db.SingleOrDefault(i => i.Id == id);
                if (msg == null)
                    throw new HttpResponseException(
                        Request.CreateResponse(HttpStatusCode.NotFound)
                    );

                return msg;
            }
        }

        public HttpResponseMessage Post_AddMessage(Message item)
        {
            lock (db)
            {
                // Add item to the "database"
                item.Id = Interlocked.Increment(ref lastId);
                db.Add(item);

                // Notify the connected clients
                Hub.Clients.addItem(item);

                // Return the new item, inside a 201 response
                var response = Request.CreateResponse(HttpStatusCode.Created, item);
                string link = Url.Link("apiRoute", new { controller = "chat", id = item.Id });
                response.Headers.Location = new Uri(link);
                return response;
            }
        }
    }
}
