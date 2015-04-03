using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Script.Serialization;

using RuneFramework;

namespace rpgSys.Controllers
{
    public class GameController : ApiController
    {
        [ActionName("create")]
        public IHttpActionResult New([FromBody]string value)
        {
            Game Game = new JavaScriptSerializer().Deserialize<Game>(value);
            return Ok(GameProcessing.Write(Game));
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            using (var db = new Runes.GameRune())
            {
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in db.Game)
                    if (Item.Scenario.Id != 0 && !Item.IsActive)
                        L.Add(new BadgeItem() { Id = Item.Id, Text = Item.Scenario.Title, Param1 = Item.Scenario.Recomendation });
                return Ok(L);
            }
        }

        [HttpGet]
        public IHttpActionResult Heroes(string GameId)
        {
            using (var db = new Runes.GameRune())
            {
                var Game = (Game)db.Game.QueryUniq("Id", "==", GameId);
                List<BadgeItem> L = new List<BadgeItem>();
                if (Game.Heroes != null && Game.Heroes.Count > 0)
                    foreach (var Item in Game.Heroes)
                        L.Add(new BadgeItem() { Text = Item.Name, Id = (int)Item.Sex });

                if (Game.Master != null)
                    L.Add(new BadgeItem() { Text = Game.Master.Name, Id = (int)Game.Master.Sex });
                return Ok(L);
            }
        }

        [HttpGet]
        public IHttpActionResult Connect(string GameId, string UserId)
        {
            using (var db = new Runes.GameRune())
            {
                using (var db2 = new Runes.UserRune())
                {
                    try
                    {
                        User u = db2.Users.ReferenceUniq("Id", "==", UserId);
                        int gameid = 0;
                        if (!Int32.TryParse(GameId, out gameid))
                            return Ok("false");
                        u.GameId = gameid;

                        using (var db3 = new Runes.HeroRune())
                        {
                            Hero h = null;
                            try
                            {
                                h = db3.Hero.ReferenceUniq("UserId", "==", UserId);
                            }
                            catch (ArgumentException)
                            {
                                return Ok("NoHero");
                            }

                            Game g = db.Game.ReferenceUniq("Id", "==", gameid);
                            if (g.Heroes == null)
                                g.Heroes = new List<Hero>();
                            g.Heroes.Add(h);

                            db2.SaveRune();
                            db.SaveRune();
                        }
                    }
                    catch (ArgumentException)
                    {
                        return Ok("false");
                    }
                }
            }
            return Ok("true");
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
            using (var db = new Runes.GameRune())
            {
                try
                {
                    Game g = db.Game.ReferenceUniq("Id", "==", GameId);

                    using (var db2 = new Runes.UserRune())
                    {
                        if (g.Heroes != null)
                            foreach (Hero h in g.Heroes)
                                db2.Users.ReferenceUniq("Id", "==", h.UserId).GameId = 0;
                        db2.Users.ReferenceUniq("Id", "==", g.Master.UserId).GameId = 0;

                        db2.SaveRune();
                    }

                    db.Game.Remove(g);
                    return Ok("true");
                }
                catch (ArgumentException) { return Ok("false"); }
            }
        }

        [HttpGet]
        public IHttpActionResult Exit(string GameId, string UserId)
        {
            int gameid = 0;
            if (!Int32.TryParse(GameId, out gameid))
                return Ok("false");

            using (var db = new Runes.GameRune())
            {
                using (var db2 = new Runes.UserRune())
                {
                    try
                    {
                        db2.Users.ReferenceUniq("Id", "==", UserId).GameId = 0;

                        using (var db3 = new Runes.HeroRune())
                        {
                            Hero h = null;
                            try
                            {
                                h = db3.Hero.ReferenceUniq("UserId", "==", UserId);
                            }
                            catch (ArgumentException)
                            {
                                return Ok("NoHero");
                            }

                            Game g = db.Game.ReferenceUniq("Id", "==", gameid);
                            if (g.Heroes == null)
                                g.Heroes = new List<Hero>();
                            g.Heroes.Remove(h);

                            db2.SaveRune();
                            db.SaveRune();
                        }
                    }
                    catch (ArgumentException)
                    {
                        return Ok("false");
                    }
                }
            }
            return Ok("true");
        }

        [HttpGet]
        public IHttpActionResult ById(string Id)
        {
            using (var db = new Runes.GameRune())
            {
                return Ok((db.Game.QueryUniq("Id", "==", Id) as Game) ?? new Game());
            }
        }
        
        [HttpGet]
        public IHttpActionResult Chat(string GameId,string Limit)
        {
            Int32 gameId = 0, limit = 100;
            if(!Int32.TryParse(GameId,out gameId))
                return Ok(new List<GameChatMessage>());
            Int32.TryParse(Limit, out limit);

            using (var db = new Runes.GameRune())
            {
                List<GameChatMessage> ChatLog = (from a in db.Chat where a.GameId == gameId select a).Take(limit).ToList();
                return Ok(ChatLog);
            }
        }
    }

    public static class GameProcessing
    {
        public static Game Write(Game G)
        {
            using (var db = new Runes.GameRune())
            {
                User U = (User)new Runes.UserRune().Users.QueryUniq("Id", "==", G.Master.UserId);
                Hero H = (Hero)db.Heroes.QueryUniq("Id", "==", U.HeroId);

                G.Master = H;

                Scenario S = (Scenario)db.Scenario.QueryUniq("Id", "==", G.Scenario.Id);

                G.Scenario = S;

                G.Npcs = S.Npcs;
                G.Event = S.Events[0];
                G.Location = S.Locations[0];

                db.Game.Add(G);
                db.SaveRune();

                using (var db2 = new Runes.UserRune())
                    foreach (var user in db2.Users)
                        if (user.Id == G.Master.UserId)
                        {
                            user.GameId = G.Id;
                            db2.SaveRune();
                        }

                return G;
            }
        }
    }
}