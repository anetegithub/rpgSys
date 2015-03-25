using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rpgSys.Controllers
{
    public class BugController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List()
        {
            using (var db = new Runes.ServerRune())
            {
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in db.Bugs)
                    L.Add(new BadgeItem() { Text = Item.Title, Badge = Item.Open == true ? "Открыт" : "Закрыт" });
                return Ok(L);
            }
        }
    }
}