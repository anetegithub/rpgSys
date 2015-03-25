using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rpgSys.Controllers
{
    public class ModuleController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List()
        {
            using (var db = new Runes.ServerRune())
            {
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in db.Modules)
                    L.Add(new BadgeItem() { Text = Item.Name, Badge = Item.Active == true ? "Подключен" : "Отключен" });
                return Ok(L);
            }
        }
    }
}
