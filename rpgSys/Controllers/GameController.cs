﻿using System;
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
            //ScenarioProcessing.Processing(Scenario);
            //return "Сценарий отправлен на рассмотрение.\nЕсли на сервере включена ручная проверка сценариев, тогда сценарий будет доступен только после проверки.";
            return Ok("true");
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            using (var db = new Runes.GameRune())
            {
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in db.Game)
                    if (Item.Scenario.Id != 0 && !Item.IsActive)
                        L.Add(new BadgeItem() { Id = Item.Id, Text = Item.Scenario.Title });
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

    public static class GameProcessing
    {
        public bool Write(Game G)
        {
            using(var db=new Runes.GameRune())
            {
                User U=new Runes.UserRune().Users.QueryUniq(new RuneBook(){ Spells=new List<RuneSpell>(){ new Spel}})
            }
        }
    }
}
