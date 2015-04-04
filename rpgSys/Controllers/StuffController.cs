using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Web.Script.Serialization;

namespace rpgSys.Controllers
{
    public class StuffController : ApiController
    {
        [ActionName("newitem")]
        public IHttpActionResult Add([FromBody]String ItemJSON)
        {
            Stuff ItemCSharp = new JavaScriptSerializer().Deserialize<Stuff>(ItemJSON);
            try { StuffProcessing.Write(ItemCSharp); }
            catch { return Ok("false"); }
            return Ok("true");
        }

        [HttpGet]
        public IHttpActionResult Item(String iid)
        {
            using (var InhRune = new Runes.HeroStuffRune())
            {
                return Ok((Stuff)InhRune.Items.QueryUniq("Id", "==", iid));
            }
        }

        [HttpGet]
        public IHttpActionResult Inventory(String hid)
        {
            using (var db = new Runes.HeroStuffRune())
            {
                return Ok((((Hero)db.Hero.QueryUniq("Id", "==", hid)) ?? new Hero()).Items ?? new List<Stuff>());
            }
        }

        [ActionName("dress")]
        public IHttpActionResult Dress([FromBody]String Instructions)
        {
            StuffInstructions Instr = new JavaScriptSerializer().Deserialize<StuffInstructions>(Instructions);

            if (Instr.UserId != 0 && Instr.HeroId != 0 && Instr.ItemId != 0)
                return DressUnDress(Instr, true, false);
            else
                return Ok("false");
        }

        [ActionName("undress")]
        public IHttpActionResult UnDress([FromBody]String Instructions)
        {
            StuffInstructions Instr = new JavaScriptSerializer().Deserialize<StuffInstructions>(Instructions);

            if (Instr.UserId != 0 && Instr.HeroId != 0 && Instr.ItemId != 0)
                return DressUnDress(Instr, false, true);
            else
                return Ok("false");
        }

        private IHttpActionResult DressUnDress(StuffInstructions Instr, Boolean Dress, Boolean UnDress)
        {
            if (Instr.Dress)
            {
                using (var db = new Runes.HeroStuffRune())
                {
                    try
                    {
                        //reference cuz Rune compare current with file
                        //if query then file will compare with current, but current will be null then loaded from file
                        //so, it will be file.Compare(itself)

                        var A = (Hero)db.Hero.ReferenceUniq("Id", "==", Instr.HeroId);
                        var B = (Stuff)db.Items.ReferenceUniq("Id", "==", Instr.ItemId);

                        var Reference = (from a in A.Items where a.Id == B.Id select a).ToList();
                        if (Reference.Count != 0)
                            if (Dress)
                                B.Dress(A);
                            else if (UnDress)
                                B.UnDress(A);

                        db.SaveRune();
                    }
                    catch (ArgumentNullException) { return Ok("false"); }
                }
                return Ok("true");
            }
            return Ok("false");
        }
    }

    internal static class StuffProcessing
    {
        internal static void Write(Stuff Item)
        {
            using (var db = new Runes.HeroStuffRune())
            {
                if (Item.Class != "")
                    Item.Class = db.Class.ReferenceUniq("Value", "==", Item.Class.Value);
                if (Item.Race != "")
                    Item.Race = db.Race.ReferenceUniq("Value", "==", Item.Race.Value);
                if (Item.Height != "")
                    Item.Height = db.Height.ReferenceUniq("Value", "==", Item.Height.Value);
                if (Item.Sex != "")
                    Item.Sex = db.Sex.ReferenceUniq("Value", "==", Item.Sex.Value);

                if (Item.Characteristics != null)
                    for (int i = 0; i < Item.Characteristics.Count; i++)
                        if (Item.Characteristics[i].Id == 0)
                            db.Characteristics.Add(Item.Characteristics[i]);
                        else
                            Item.Characteristics[i] = (Characteristic)db.Characteristics.QueryUniq("Id", "==", Item.Characteristics[i].Id);

                if (Item.Abilities != null)
                    for (int i = 0; i < Item.Abilities.Count; i++)
                        if (Item.Abilities[i].Id == 0)
                            db.Abilities.Add(Item.Abilities[i]);
                        else
                            Item.Abilities[i] = (Ability)db.Abilities.QueryUniq("Id", "==", Item.Abilities[i].Id);

                if (Item.Skills != null)
                    for (int i = 0; i < Item.Skills.Count; i++)
                        if (Item.Skills[i].Id == 0)
                            db.Skills.Add(Item.Skills[i]);
                        else
                            Item.Skills[i] = (Skill)db.Skills.QueryUniq("Id", "==", Item.Skills[i].Id);

                if (Item.HealthState.Id == 0)
                    db.HealthState.Add(Item.HealthState);
                else
                    Item.HealthState = (HealthState)db.HealthState.QueryUniq("Id", "==", Item.HealthState.Id);

                if (Item.DefenceState.Id == 0)
                    db.DefenceState.Add(Item.DefenceState);
                else
                    Item.DefenceState = (DefenceState)db.DefenceState.QueryUniq("Id", "==", Item.DefenceState.Id);

                if (Item.AttackState.Id == 0)
                    db.AttackState.Add(Item.AttackState);
                else
                    Item.AttackState = (AttackState)db.AttackState.QueryUniq("Id", "==", Item.AttackState.Id);

                if (Item.CommonState.Id == 0)
                    db.CommonState.Add(Item.CommonState);
                else
                    Item.CommonState = (CommonState)db.CommonState.QueryUniq("Id", "==", Item.CommonState.Id);

                db.Items.Add(Item);
            }
        }
    }
}