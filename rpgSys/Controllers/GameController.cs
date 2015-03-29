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
        public IHttpActionResult ById(string Id)
        {
            using (var db = new Runes.GameRune())
            {
                return Ok((db.Game.QueryUniq(new RuneSpell("Id", "==", Id)) as Game) ?? new Game());
            }
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
                var Game = (Game)db.Game.QueryUniq(new RuneFramework.RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", GameId) } });
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
            bool Success = false;
            using(var db=new Runes.UserRune())
            {
                //(db.Users.QueryUniq(new RuneSpell("Id", "==", UserId)) as User).GameId = 666;

                //(from a in db.Users where a.Id == Int32.Parse(UserId) select a).ToList()[0].GameId = 666;

                db.Users.Reference(new SimpleRuneBook() { Spells = new List<SimpleRuneSpell>() { new SimpleRuneSpell("Id", "==", UserId), new SimpleRuneSpell("Id", "==", UserId) } })[0].GameId = 1;


                db.SaveRune();

            }
            //using (var db = new Runes.GameRune())
            //{
            //    using (var dbh = new Runes.HeroRune())
            //    {
            //        foreach (Hero H in dbh.Hero)
            //            if (H.UserId == Int32.Parse(UserId))
            //                foreach (Game G in db.Game)
            //                    if (G.Id == Int32.Parse(GameId))
            //                    {
            //                        G.Heroes.Add(H);
            //                        Success = !Success;
            //                    }
            //    }
            //}
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
            int indexOf = -1;
            using (var db = new Runes.GameRune())
            {
                foreach (Game G in db.Game)
                    if (G.Id == Int32.Parse(GameId))
                        indexOf = db.Game.ToList().IndexOf(G);
                if (indexOf != -1)
                    db.Game.Remove(db.Game[indexOf]);
            }
            if (indexOf != -1)
                return Ok("true");
            else
                return Ok("false");
        }
    }

    public static class GameProcessing
    {
        public static Game Write(Game G)
        {
            using (var db = new Runes.GameRune())
            {
                User U = (User)new Runes.UserRune().Users.QueryUniq(new RuneSpell("Id", "==", G.Master.UserId));
                Hero H = (Hero)db.Heroes.QueryUniq(new RuneSpell("Id", "==", U.HeroId));

                G.Master = H;

                Scenario S = (Scenario)db.Scenario.QueryUniq(new RuneSpell("Id", "==", G.Scenario.Id));

                G.Scenario = S;

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