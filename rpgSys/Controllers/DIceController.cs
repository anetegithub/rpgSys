using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rpgSys.Controllers
{
    /// <summary>
    /// d20 + modifiers vs. Difficulty Class
    /// </summary>
    public class DiceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Throw(String dice, String modifer, String difficult)
        {
            Int32 Dice = 0, Modifer = 0, Difficult = 0;

            if (!Int32.TryParse(dice, out Dice)) return Ok(HttpStatusCode.BadRequest);
            if (!Int32.TryParse(modifer, out Modifer)) return Ok(HttpStatusCode.BadRequest);
            if (!Int32.TryParse(difficult, out Difficult)) return Ok(HttpStatusCode.BadRequest);

            if (Dice < 2)
                return Ok(HttpStatusCode.BadRequest);
            else
                return Ok(new Dice(Dice).Throw(Difficult, Modifer));
        }
    }

    public class Dice
    {
        private static Random Random = new Random();

        public Dice() { }
        public Dice(Int32 Edges)
        { this.Edges = Edges; }

        public Int32 Edges = 0;

        public Throw Throw(Int32 Difficult, Int32 Modifer)
        {
            Throw Throw = new Throw();
            Throw.Modifer = Modifer;
            Throw.Difficult = Difficult;
            Throw.Result = Random.Next(Edges+1);
            Throw.Win = Throw.Result >= Difficult ? true : false;
            return Throw;
        }
    }

    public sealed class Throw
    {
        public Int32 Result = 0;
        public Int32 Modifer = 0;
        public Int32 Difficult = 0;
        public Boolean Win = false;
    }
}