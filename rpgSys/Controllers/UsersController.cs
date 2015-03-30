using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

using System.Dynamic;


using RuneFramework;

namespace rpgSys
{
    public class UsersController : ApiController
    {
        public IHttpActionResult Get(string name, string psw)
        {
            Rune.Element = RuneElement.Air;

            using (var db = new Runes.UserRune())
            {
                User user = (User)db.Users.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Login", "==", name), new RuneSpell("Password", "==", psw) } });
                if (user != null)
                {
                    user.Stamp = DateTime.Parse(user.Stamp).Ago();
                    return Ok(user);
                }
            }
            return Conflict();
        }

        [ActionName("update")]
        [HttpPost]
        public string UpdateUserStamp([FromBody]string UserId)
        {
            int id = 0;
            if (!Int32.TryParse(UserId, out id))
                return "False";

            using (var db = new Runes.UserRune())
            {
                foreach (User u in db.Users)
                {
                    if (u.Id == id)
                    {
                        u.Stamp = DateTime.Now.ToString();
                        db.SaveRune();
                        return "True";
                    }
                }
            }

            return "False";
        }

        [ActionName("create")]
        [HttpPut]
        public string CreateUser([FromBody]string User)
        {
            User user = new JavaScriptSerializer().Deserialize<User>(User);
            user.Stamp = DateTime.Now.ToString();

            using(var db=new Runes.UserRune())
            {
                if (db.Users.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Login", "==", user.Login) } }) != null)
                    return "Такой пользователь уже существует!";
                else
                {
                    user.Avatar = "img/unknown.png";
                    db.Users.Add(user);
                    db.Activity.Add(new UserActivity() { Action = "1", Stamp = DateTime.Now.ToString(), Text = "Вы успешно зарегистрировались!" });
                    db.Users.Last().Activity = new List<UserActivity>() { db.Activity.Last() };
                    db.SaveRune();
                    return "True";
                }
            }
        }
    }
}