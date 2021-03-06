﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Script.Serialization;

using System.Diagnostics;
using System.Web;

using RuneFramework;
using rpgSys.Log;

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

        [ActionName("chgnpc")]
        public IHttpActionResult Npc([FromBody]String value)
        {
            Answer Request = new Answer();

            Logger.LookAfter(() =>
                {
                    Request = new JavaScriptSerializer().Deserialize<Answer>(value);
                });

            if (Request.Id != 0 && Request.GameId != 0)
                using (var db = new Runes.GameRune())
                {
                    if (db.Game.ReferenceUniq("Id", "==", Request.GameId).Npcs == null)
                        db.Game.ReferenceUniq("Id", "==", Request.GameId).Npcs = new List<Npc>();

                    if (Request.Set)
                        db.Game.ReferenceUniq("Id", "==", Request.GameId).Npcs.Add(db.Npcs.QueryUniqSafe("Id", "==", Request.Id));
                    else
                    {
                        Game RuneBug = (Game)db.Game.ReferenceUniq("Id", "==", Request.GameId);
                        if (RuneBug.Npcs == null)
                            RuneBug.Npcs = new List<Npc>();
                        RuneBug.Npcs.Remove(RuneBug.Npcs.Where(x => x.Id == Request.Id).ToList()[0]);
                        
                    }

                    db.SaveRune();

                    return Ok("true");
                }
            else
                return Ok("false");
        }

        [ActionName("chgevent")]
        public IHttpActionResult Event([FromBody]String value)
        {
            Answer Request = new Answer();

            Logger.LookAfter(() =>
            {
                Request = new JavaScriptSerializer().Deserialize<Answer>(value);
            });

            if (Request.Id != 0 && Request.GameId != 0)
                using (var db = new Runes.GameRune())
                {
                    db.Game.ReferenceUniq("Id", "==", Request.GameId).Event = db.Events.ReferenceUniq("Id", "==", Request.Id);
                    db.SaveRune();
                    return Ok("true");
                }
            else
                return Ok("false");
        }

        [ActionName("chglocation")]
        public IHttpActionResult Location([FromBody]String value)
        {
            Answer Request = new Answer();

            Logger.LookAfter(() =>
            {
                Request = new JavaScriptSerializer().Deserialize<Answer>(value);
            });

            if (Request.Id != 0 && Request.GameId != 0)
                using (var db = new Runes.GameRune())
                {
                    db.Game.ReferenceUniq("Id", "==", Request.GameId).Location = db.Locations.ReferenceUniq("Id", "==", Request.Id);
                    db.SaveRune();
                    return Ok("true");
                }
            else
                return Ok("false");
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
            Int32 gameId = 0;
            List<BadgeItem> L = new List<BadgeItem>();
            if (!Int32.TryParse(GameId, out gameId))
                return Ok("false");
            if (gameId == 0)
                return Ok(L);

            using (var db = new Runes.GameRune())
            {
                Game Game = db.Game.Find(x => x.Id == gameId);
                if (Game.Heroes != null)
                    foreach (var Hero in Game.Heroes)
                        L.Add(new BadgeItem() { Text = Hero.Name, Id = (Int32)Hero.Sex });
                L.Add(new BadgeItem() { Text = Game.Master.Name, Id = (Int32)Game.Master.Sex });
            }

            return Ok(L);
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
            Int32 gameId = 0;
            if (!(Int32.TryParse(GameId, out gameId)))
                return Ok("false");

            using (var db = new Runes.GameCommunicationRune())
            {
                Game g = db.Game.ReferenceUniq("Id", "==", gameId);
                if (g == null)
                    return Ok("false");

                if (g.Heroes != null)
                    foreach (var h in g.Heroes)
                        db.User.ReferenceUniq("Id","==",h.UserId).GameId = 0;
                db.User.ReferenceUniq("Id", "==", g.Master.UserId).GameId = 0;

                db.SaveRune();
                db.Game.Remove(g);
            }

            return Ok("true");
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

                        Hero h = null;
                        try
                        {
                            h = db.Heroes.ReferenceUniq("UserId", "==", UserId);
                        }
                        catch (ArgumentException)
                        {
                            return Ok("NoHero");
                        }

                        Game g = db.Game.Find(x => x.Id == gameid);
                        if (g.Heroes == null)
                            g.Heroes = new List<Hero>();

                        g.Heroes.Remove(g.Heroes.Find(x => x.Id == h.Id));

                        db2.SaveRune();
                        db.SaveRune();
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
            Game game = new Game();
            Logger.LookAfter(() =>
                {
                    using (var db = new Runes.GameRune())
                    {
                        game = (Game)db.Game.QueryUniq("Id", "==", Id);
                    }
                });
            return Ok(game);
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

    public sealed class Answer
    {
        public Int32 Id = 0, GameId = 0;
        public Boolean Set = true;
    }

    internal static class GameProcessing
    {
        internal static Game Write(Game G)
        {
            using (var db = new Runes.GameRune())
            {
                User U = (User)new Runes.UserRune().Users.QueryUniq("Id", "==", G.Master.UserId);
                Hero H = (Hero)db.Heroes.QueryUniq("Id", "==", U.HeroId);

                G.Master = H;

                Scenario S = (Scenario)db.Scenario.QueryUniq("Id", "==", G.Scenario.Id);

                G.Scenario = S;

                //G.Npcs = S.Npcs;
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