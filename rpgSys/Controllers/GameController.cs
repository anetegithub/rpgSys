using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using RuneFramework;

namespace rpgSys.Controllers
{
    public class GameController : ApiController
    {
        [HttpGet]
        public IHttpActionResult List()
        {
            using (var db = new Runes.GameRune())
            {
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in db.Game)
                    if (Item.Scenario.Id != 0 && !Item.IsActive)
                        L.Add(new BadgeItem() { Id = Item.Id, Text = Item.Scenario.Title, Badge = Item.IsActive == true ? "Активен" : "Не активен" });
                return Ok(L);
            }
        }

        [HttpGet]
        public IHttpActionResult Heroes(string GameId)
        {
            using (var db = new Runes.GameRune())
            {
                var Game = (Game)db.Game.QueryUniq(new RuneFramework.RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", GameId) } });
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in Game.Heroes)
                    L.Add(new BadgeItem() { Text = Item.Name, Id = (int)Item.Sex });
                return Ok(L);
            }
        }

        [HttpGet]
        public IHttpActionResult Connect(string GameId, string UserId)
        {
            bool Success = false;
            using (var db = new Runes.GameRune())
            {
                using (var dbh = new Runes.HeroRune())
                {
                    foreach (Hero H in dbh.Hero)
                        if (H.UserId == Int32.Parse(UserId))
                            foreach (Game G in db.Game)
                                if (G.Id == Int32.Parse(GameId))
                                {
                                    G.Heroes.Add(H);
                                    Success = !Success;
                                }
                }
            }
            if (Success)
                return Ok("true");
            else
                return Ok("false");
        }

        [HttpGet]
        public IHttpActionResult Get(string GameId)
        {
            bool Success = false;
            using (var db = new Runes.GameRune())
            {
                foreach (Game G in db.Game)
                    if (G.Id == Int32.Parse(GameId))
                    {
                        G.IsActive = true;
                        Success = !Success;
                        db.SaveRune();
                    }
            }
            if (Success)
                return Ok("true");
            else
                return Ok("false");
        }

        [HttpGet]
        public IHttpActionResult Start(string GameId)
        {
            bool Success = false;
            using (var db = new Runes.GameRune())
            {
                foreach (Game G in db.Game)
                    if (G.Id == Int32.Parse(GameId))
                    {
                        G.IsActive = true;
                        Success = !Success;
                        db.SaveRune();
                    }
            }
            if (Success)
                return Ok("true");
            else
                return Ok("false");
        }

        [HttpGet]
        public IHttpActionResult Delete(string GameId)
        {
            bool Success = false;
            using (var db = new Runes.GameRune())
            {
                foreach (Game G in db.Game)
                    if (G.Id == Int32.Parse(GameId))
                    {
                        G.IsActive = true;
                        Success = !Success;
                        db.Game.Remove(G);
                        db.SaveRune();
                    }
            }
            if (Success)
                return Ok("true");
            else
                return Ok("false");
        }
    }
}
