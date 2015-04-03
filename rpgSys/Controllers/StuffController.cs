using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rpgSys.Controllers
{
    public class StuffController : ApiController
    {
        [HttpGet]
        public IHttpActionResult ById(String id)
        {
            using(var InhRune=new Runes.HeroStuffRune())
            {
                return Ok(InhRune.Items.ToList());
            }
        }

        [HttpGet]
        public IHttpActionResult Add(String id)
        {
            using (var InhRune = new Runes.HeroStuffRune())
            {
                Stuff a = new Stuff();
                a.Height = 2;

                HealthState hs = new HealthState();
                InhRune.HealthState.Add(hs);
                a.HealthState = hs;

                AttackState asT = new AttackState();
                InhRune.AttackState.Add(asT);
                a.AttackState = asT;

                InhRune.Items.Add(a);

                InhRune.SaveRune();

                return Ok(InhRune.Items.ToList());
            }
        }
    }
}
