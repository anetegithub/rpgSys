using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

using System.Dynamic;

using ormCL;
using RuneFramework;

using System.Diagnostics;

namespace rpgSys
{
    public class UsersController : ApiController
    {
        public IHttpActionResult Get(string name, string psw)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Rune.Element = RuneElement.Air;

            LocalUser userq = new LocalUser();
            using(var db=new Models.UserRune())
            {
                //Single query                
                userq = (LocalUser)db.Users.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Login", "==", "Anete") } });

                //MassQuery
                //foreach(LocalUser userItem in db.Users)
                //{
                //    if (userItem.Login == "Anete")
                //        user = userItem;
                //}


                //LocalUser lu = new LocalUser();
                //lu.Login = "Anete";
                //lu.Password = "555033";
                //lu.Avatar = "img/admin.png";
                //lu.Email = "anete.anetes@gmail.com";
                //lu.Auth = Guid.NewGuid().ToString();

                //db.Users.Add(lu);

                //db.SaveRune();                
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);
            sw.Start();
            var users = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/User/Users") }).Cast<User>().Filter(new conditionCL("Login.==." + name + ",Password.==." + psw)).ToList();
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);
            if (users.Count == 0)
            {
                return NotFound();
            }
            else if (users.Count == 1)
            {
                var user = users[0];
                //user.StampToString = user.Stamp.Ago();
                return Ok(user);
            }
            else
            {
                return Conflict();
            }
        }

        public class LocalUser
        {
            public int Id { get; set; }
            public string Avatar { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Auth { get; set; }
        }

        [ActionName("update")]
        [HttpPost]
        public string UpdateUserStamp([FromBody]string UserId)
        {
            baseCL b = new baseCL("Data");
            User User = b.Select(new requestCL() { Table = new tableCl("/User/Users"), Conditions = new conditionCL("Id.==." + UserId) }).Cast<User>().Filter().ToList()[0];
            User.Stamp = DateTime.Now.ToString();
            var result=b.Update<User>(new urequestCl(new conditionCL("Id.==." + UserId)) { Object = User, Table = new tableCl("/User/Users") }).Successful.ToString();
            return result;
        }

        [ActionName("create")]
        [HttpPut]
        public string CreateUser([FromBody]string User)
        {
            User user = new JavaScriptSerializer().Deserialize<User>(User);
            user.Stamp = DateTime.Now.ToString();

            using(var db=new Models.UserRune())
            {
                if (db.Users.QueryUniq(new RuneBook() { Spells = new List<RuneSpell>() { new RuneSpell("Login", "==", user.Login) } }) != null)
                    return "Такой пользователь уже существует!";
                else
                {
                    db.Users.Add(user);
                    db.SaveRune();
                    return "True";
                }
            }

            //user.Auth = Guid.NewGuid().ToString();
            //user.GameId = 0;
            //user.HeroId = 0;
            baseCL b = new baseCL("Data");
            tableCl t = new tableCl("/User/Users");
            var users=b.Select(new requestCL() { Table = t }).Cast<User>().Filter(new conditionCL("Login.!=." + user.Login)).ToList();
            if (users.Count== 0)
            {
                user.Id = b.Select(new requestCL() { Table = t }).Cast<User>().ToList().Count + 1;
                return b.Insert<User>(new irequestCl() { Table = t, Object = user }).Successful.ToString();
            }
            else
            {
                return "Пользователь с таким именем уже существует!";
            }
        }
    }

    public class Game
    {
        public int ScriptId;
    }
}