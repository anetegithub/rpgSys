using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ormCL;

using RuneFramework;

namespace rpgSys
{
    public class ActivityController : ApiController
    {
        public IHttpActionResult Get()
        {
            List<UserActivity> Activities = new List<UserActivity>();

            using (var db = new Runes.UserRune())
            {
                foreach (var Activity in db.Activity)
                {
                    Activities.Add(Activity);
                }
            }
            return Ok(Activities);
        }

        public IHttpActionResult Get(int Id)
        {
            using (var db = new Runes.UserRune())
            {
                var user = (db.Users.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Id", "==", Id) } }));
                if (user != null)
                {
                    foreach(var A in (user as User).Activity)
                    {
                        A.Stamp = DateTime.Parse(A.Stamp).Ago();
                    }
                    return Ok((user as User).Activity);
                }
                else
                    return InternalServerError();
            }
        }

        [ActionName("add")]
        public bool Put([FromBody] string Settings)
        {
            return false;
        }
    }
}