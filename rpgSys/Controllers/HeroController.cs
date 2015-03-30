using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;

using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;



using RuneFramework;

namespace rpgSys.Controllers
{
    public class HeroController : ApiController
    {
        [ActionName("create")]
        public string NewCharacter([FromBody]string value)
        {
            Hero NewHero = new JavaScriptSerializer().Deserialize<Hero>(value);
            return HeroProcessing.Processing(NewHero).ToString();
        }

        [HttpGet]
        public IHttpActionResult List()
        {
            using (var db = new Runes.HeroRune())
            {
                List<BadgeItem> L = new List<BadgeItem>();
                foreach (var Item in db.Hero)
                    L.Add(new BadgeItem() { Text = Item.Name, Badge = Item.Active == true ? "Активен" : "Не активен" });
                return Ok(L);
            }
        }

        public IHttpActionResult Get(string UserId)
        {
            Hero FindHero = null;

            using (var db = new Runes.HeroRune())
            {
                FindHero = (Hero)db.Hero.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("UserId", "==", UserId) } });
            }

            if (FindHero != null)
                return Ok(FindHero);
            else
                return NotFound();
        }

        [HttpGet]
        public IHttpActionResult Enums()
        {
            List<List<RuneString>> Enums = new List<List<RuneString>>();
            using (var db = new Runes.HeroRune())
            {
                Enums.Add(db.Class.ToList());
                Enums.Add(db.Race.ToList());
                Enums.Add(db.Sex.ToList());
                Enums.Add(db.Height.ToList());
            }
            return Ok(Enums);
        }

        [HttpGet]
        public IHttpActionResult HealthState()
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok(db.HealthState.ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult HealthState(string hsId)
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok((HealthState)db.HealthState.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", hsId) } }));
            }
        }

        [HttpGet]
        public IHttpActionResult Skills()
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok(db.Skills.ToList());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skId">Hero Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Skills(string skId)
        {
            using (var db = new Runes.HeroRune())
            {
                Hero h = (Hero)db.Hero.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", skId) } });
                if (h != null)
                    return Ok(h.Skills ?? new List<Skill>());
                else
                    return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult DefenceState()
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok(db.DefenceState.ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult DefenceState(string dfId)
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok((DefenceState)db.DefenceState.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", dfId) } }));
            }
        }

        [HttpGet]
        public IHttpActionResult AttackState()
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok(db.AttackState.ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult AttackState(string atId)
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok((AttackState)db.AttackState.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", atId) } }));
            }
        }

        [HttpGet]
        public IHttpActionResult CommonState()
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok(db.CommonState.ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult CommonState(string cmId)
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok((CommonState)db.CommonState.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", cmId) } }));
            }
        }

        [HttpGet]
        public IHttpActionResult Characteristics()
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok(db.Characteristics.ToList());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chId">Hero Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Characteristics(string chId)
        {
            using (var db = new Runes.HeroRune())
            {
                Hero h = (Hero)db.Hero.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", chId) } });
                if (h != null)
                    return Ok(h.Characteristics ?? new List<Characteristic>());
                else
                    return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult Ability()
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok(db.Abilities.ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult Ability(string abId)
        {
            using (var db = new Runes.HeroRune())
            {
                return Ok((Ability)db.Abilities.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", abId) } }));
            }
        }
    }

    public static class HeroProcessing
    {
        public static int Processing(Hero Hero)
        {
            int userId = Hero.UserId;
            MainProcessing(Hero,userId);
            Ability(userId);
            HealthState(userId);
            DefenceState(userId);
            AttackState(userId);
            CommonState(userId);

            return Hero.Id;
        }

        private static void MainProcessing(Hero Hero,int userId)
        {
            using (var db = new Runes.HeroRune())
            {
                foreach (var Item in db.Class)
                    if (Item.Value == Hero.Class.Value)
                        Hero.Class = Item;

                foreach (var Item in db.Race)
                    if (Item.Value == Hero.Race.Value)
                        Hero.Race = Item;

                foreach (var Item in db.Height)
                    if (Item.Value == Hero.Height.Value)
                        Hero.Height = Item;

                foreach (var Item in db.Sex)
                    if (Item.Value == Hero.Sex.Value)
                        Hero.Sex = Item;

                foreach (var S in Hero.Skills)
                {
                    foreach (var Item in db.SkillName)
                        if (Item.Value == S.SkillName.Value)
                            S.SkillName = Item;

                    foreach (var Item in db.DIX)
                        if (Item.Value == S.DIX.Value)
                            S.DIX = Item;

                    db.Skills.Add(S);
                }

                foreach (var C in Hero.Characteristics)
                {
                    foreach (var Item in db.CharacteristicName)
                        if (Item.Value == C.CharacteristicName.Value)
                            C.CharacteristicName = Item;

                    foreach (var Item in db.DIX)
                        if (Item.Value == C.DIX.Value)
                            C.DIX = Item;

                    db.Characteristics.Add(C);                    
                }

                db.Characteristics.Add(new Characteristic() { CharacteristicName = 6, DIX = 6, Value = 0 });

                Hero.Characteristics.Add(db.Characteristics.Last());

                db.Hero.Add(Hero);
                db.SaveRune();

                using (var idb = new Runes.UserRune())
                {
                    var user = (from a in idb.Users.ToList() where a.Id == userId select a).ToList()[0];
                    user.HeroId = ((Hero)db.Hero.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("UserId", "==", userId) } })).Id;
                    idb.SaveRune();
                }
            }
        }

        private static void Ability(int userId)
        {
            using (var db = new Runes.HeroRune())
            {
                foreach (Hero Hero in db.Hero)
                {
                    if (Hero.UserId == userId)
                    {
                        if (Hero.Abilities == null)
                            Hero.Abilities = new List<rpgSys.Ability>();

                        Ability Fortitude = new Ability();
                        Fortitude.AbilityName = 1;
                        Fortitude.Value = (Hero.Characteristics[0].Value / 2) + (Hero.Characteristics[1].Value / 2);

                        db.Abilities.Add(Fortitude);
                        Hero.Abilities.Add(Fortitude);

                        Ability Reflex = new Ability();
                        Reflex.AbilityName = 2;
                        Reflex.Value = (Hero.Characteristics[1].Value / 2);
                        db.Abilities.Add(Reflex);
                        Hero.Abilities.Add(Reflex);

                        Ability Will = new Ability();
                        Will.AbilityName = 3;
                        Will.Value = (Hero.Characteristics[3].Value / 2) + (Hero.Characteristics[2].Value / 4);
                        db.Abilities.Add(Will);
                        Hero.Abilities.Add(Will);

                        db.SaveRune();
                    }
                }
            }
        }

        private static void HealthState(int userId)
        {
            using (var db = new Runes.HeroRune())
            {
                foreach (Hero Hero in db.Hero)
                {
                    if (Hero.UserId == userId)
                    {
                        HealthState State = new HealthState();
                        State.CurrentHitPoints = Hero.Characteristics[0].Value * 2;
                        State.MaximumHitPoints = Hero.Characteristics[0].Value * 2;

                        State.Desease = db.Desease[0];
                        State.Intoxication = db.Intoxication[0];
                        State.Charm = db.Charm[0];
                        db.HealthState.Add(State);

                        Hero.HealthState = State;
                        db.SaveRune();
                    }
                }
            }
        }

        private static void DefenceState(int userId)
        {
            using (var db = new Runes.HeroRune())
            {
                foreach (Hero Hero in db.Hero)
                {
                    if (Hero.UserId == userId)
                    {
                        DefenceState State = new DefenceState();
                        State.NaturalDefence = Hero.Characteristics[0].Value / 4;
                        State.Defence = State.NaturalDefence;
                        db.DefenceState.Add(State);

                        Hero.DefenceState = State;
                        db.SaveRune();
                    }
                }
            }
        }

        private static void AttackState(int userId)
        {
            using (var db = new Runes.HeroRune())
            {
                foreach (Hero Hero in db.Hero)
                {
                    if (Hero.UserId == userId)
                    {
                        AttackState State = new AttackState();
                        State.FitAttack = Hero.Characteristics[1].Value / 4;
                        State.Attack = State.FitAttack;

                        State.MinimalDamage = 1 + (State.Attack / 2);
                        State.MaximalDamage = 2 + (int)((State.Attack + 0.5) / 2);

                        State.CritChance = 5;
                        State.CritBonus = 5;
                        db.AttackState.Add(State);

                        Hero.AttackState = State;
                        db.SaveRune();
                    }
                }
            }
        }

        private static void CommonState(int userId)
        {
            using (var db = new Runes.HeroRune())
            {
                foreach (Hero Hero in db.Hero)
                {
                    if (Hero.UserId == userId)
                    {
                        CommonState State = new CommonState();
                        State.SpeedFit = Hero.Characteristics[1].Value / 4;
                        State.Speed = 3 + State.SpeedFit;

                        if ((int)Hero.Height == 2)
                            State.InitiativeSize = 1;
                        if (Hero.Characteristics[3].Value > 5)
                            State.InitiativeWisdom = 1;

                        State.Initiative = (Hero.Characteristics[2].Value / 3) + State.InitiativeSize + State.InitiativeWisdom;
                        db.CommonState.Add(State);

                        Hero.CommonState = State;

                        db.SaveRune();
                    }
                }
            }
        }
    }
}