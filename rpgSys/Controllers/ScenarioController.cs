using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.IO;

using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

using System.Drawing;

using System.Web;

using RuneFramework;

namespace rpgSys.Controllers
{
    public class ScenarioController : ApiController
    {
        [ActionName("create")]
        public string NewScenario([FromBody]string value)
        {
            Scenario Scenario = new JavaScriptSerializer().Deserialize<Scenario>(value);
            ScenarioProcessing.Processing(Scenario);
            return "Сценарий отправлен на рассмотрение.\nЕсли на сервере включена ручная проверка сценариев, тогда сценарий будет доступен только после проверки.";
        }

        [HttpGet]
        public IHttpActionResult Enums()
        {
            List<List<RuneString>> Enums = new List<List<RuneString>>();
            using (var db = new Runes.ScenarioRune())
            {
                Enums.Add(db.Target.ToList());
                Enums.Add(db.RewardInfo.ToList());
                Enums.Add(db.Rare.ToList());
            }
            return Ok(Enums);
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            using (var db = new Runes.ScenarioRune())
            {
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in db.Scenarios)
                    L.Add(new BadgeItem() { Text = Item.Title, Badge = Item.Active == true ? "Активен" : "Не активен" });
                return Ok(L);
            }
        }

    }

    public static class ScenarioProcessing
    {
        public static void Processing(Scenario Scenario)
        {
            using (var db = new Runes.ScenarioRune())
            {
                foreach (var Location in Scenario.Locations)
                {
                    if (Location.Map != "")
                        Location.Map = SaveMap(Location);

                    db.Locations.Add(Location);
                }

                foreach (var Event in Scenario.Events)
                    db.Events.Add(Event);

                foreach (var Npc in Scenario.Npcs)
                {
                    db.NpcStats.Add(Npc.NpcStats);
                    db.Npcs.Add(Npc);
                }

                foreach (var Reward in Scenario.Rewards)
                {
                    foreach (var RewardStat in Reward.RewardStats)
                    {
                        foreach (var Info in db.RewardInfo)
                            if (Info.Value == RewardStat.RewardInfo.Value)
                                RewardStat.RewardInfo = Info;

                        db.RewardStats.Add(RewardStat);
                    }

                    foreach (var Rare in db.Rare)
                        if (Rare.Value == Reward.Rare.Value)
                            Reward.Rare = Rare;

                    foreach (var Target in db.Target)
                        if (Target.Value == Reward.Target.Value)
                            Reward.Target = Target;
                    db.Rewards.Add(Reward);
                }

                db.Scenarios.Add(Scenario);
            }
        }

        private static string SaveMap(Location Map)
        {
            //get a temp image from bytes, instead of loading from disk
            //data:image/gif;base64,
            //this image is a single pixel (black)            
            byte[] bytes = Convert.FromBase64String(Map.Map.Replace("data:image/png;base64,", ""));

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            string path = HttpContext.Current.Server.MapPath("~/site/img/" + ConvertToTranslit(Map.Name) + ".png");
            image.Save(path);

            return path;
        }

        private static string ConvertToTranslit(string Source)
        {
            Init();
            string Result = "";
            foreach (char c in Source)
                Result += transliter[c.ToString()];
            return Result;
        }

        private static Dictionary<string, string> transliter = new Dictionary<string, string>();
        private static bool inited = false;
        private static void Init()
        {
            if (!inited)
            {
                inited = true;
                transliter.Add("а", "a");
                transliter.Add("б", "b");
                transliter.Add("в", "v");
                transliter.Add("г", "g");
                transliter.Add("д", "d");
                transliter.Add("е", "e");
                transliter.Add("ё", "yo");
                transliter.Add("ж", "zh");
                transliter.Add("з", "z");
                transliter.Add("и", "i");
                transliter.Add("й", "j");
                transliter.Add("к", "k");
                transliter.Add("л", "l");
                transliter.Add("м", "m");
                transliter.Add("н", "n");
                transliter.Add("о", "o");
                transliter.Add("п", "p");
                transliter.Add("р", "r");
                transliter.Add("с", "s");
                transliter.Add("т", "t");
                transliter.Add("у", "u");
                transliter.Add("ф", "f");
                transliter.Add("х", "h");
                transliter.Add("ц", "c");
                transliter.Add("ч", "ch");
                transliter.Add("ш", "sh");
                transliter.Add("щ", "sch");
                transliter.Add("ъ", "j");
                transliter.Add("ы", "i");
                transliter.Add("ь", "j");
                transliter.Add("э", "e");
                transliter.Add("ю", "yu");
                transliter.Add("я", "ya");
                transliter.Add("А", "A");
                transliter.Add("Б", "B");
                transliter.Add("В", "V");
                transliter.Add("Г", "G");
                transliter.Add("Д", "D");
                transliter.Add("Е", "E");
                transliter.Add("Ё", "Yo");
                transliter.Add("Ж", "Zh");
                transliter.Add("З", "Z");
                transliter.Add("И", "I");
                transliter.Add("Й", "J");
                transliter.Add("К", "K");
                transliter.Add("Л", "L");
                transliter.Add("М", "M");
                transliter.Add("Н", "N");
                transliter.Add("О", "O");
                transliter.Add("П", "P");
                transliter.Add("Р", "R");
                transliter.Add("С", "S");
                transliter.Add("Т", "T");
                transliter.Add("У", "U");
                transliter.Add("Ф", "F");
                transliter.Add("Х", "H");
                transliter.Add("Ц", "C");
                transliter.Add("Ч", "Ch");
                transliter.Add("Ш", "Sh");
                transliter.Add("Щ", "Sch");
                transliter.Add("Ъ", "J");
                transliter.Add("Ы", "I");
                transliter.Add("Ь", "J");
                transliter.Add("Э", "E");
                transliter.Add("Ю", "Yu");
                transliter.Add("Я", "Ya");
            }
        }
    }

    public class BadgeItem
    {
        public string Text { get; set; }
        public string Badge { get; set; }
    }
}