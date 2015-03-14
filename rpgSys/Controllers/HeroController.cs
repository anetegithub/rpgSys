using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;

using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

using ormCL;

namespace rpgSys.Controllers
{
    public class HeroController : ApiController
    {
        /// <summary>
        /// WARNING!!! METHOD BLOCKING ALL TIME!!!
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ActionName("create")]
        public string NewCharacter([FromBody]string value)
        {
            Hero NewHero = new JavaScriptSerializer().Deserialize<Hero>(value);
            var result= new baseCL("Data").Insert<Hero>(new irequestCl() { Table = new tableCl("/Hero/Info"), Object = NewHero }).Successful.ToString();
            return result;
        }

        public IHttpActionResult GetInfo(string HeroId)
        {
            List<Hero> Heroes = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/Hero/Info") }).Cast<Hero>().Filter(new conditionCL("Id.==." + HeroId)).ToList();
            if (Heroes.Count > 0)
            {
                return Ok(Heroes[0]);
            }
            else
            {
                return NotFound();
            }

            //if (xmlBase.Users.Authed(UserId, Auth))
            //{
            //    switch (OperationId.Split('~')[0])
            //    {
            //        case "0": return info(UserId);
            //        case "1": return characteristics(UserId);
            //        case "2": return proficiency(UserId);
            //        case "3": return healthstate(UserId);
            //        case "4": return defencestate(UserId);
            //        case "5": return attackstate(UserId);
            //        case "6": return initiativestate(UserId);
            //        case "7": return materialskill(UserId);
            //        case "8": return mentalskill(UserId);
            //        case "9": return classskill(UserId);
            //        case "10": return getskills("Material");
            //        case "11": return getskills("Mental");
            //        case "12": return getskills(OperationId.Split('~')[1]);
            //        default: return NotFound();
            //    }
            //}
            //else
            //{
            //    return BadRequest("Ключ авторизации истёк!");
            //}
        }

        public IHttpActionResult GetEnums()
        {
            try
            {
                List<List<Enums>> List = new List<List<Enums>>();
                baseCL b = new baseCL("Data");
                List.Add(b.Select(new requestCL() { Table = new tableCl("/Hero/Common/Class") }).Cast<Enums>().ToList());
                List.Add(b.Select(new requestCL() { Table = new tableCl("/Hero/Common/Race") }).Cast<Enums>().ToList());
                List.Add(b.Select(new requestCL() { Table = new tableCl("/Hero/Common/Sex") }).Cast<Enums>().ToList());
                List.Add(b.Select(new requestCL() { Table = new tableCl("/Hero/Common/Height") }).Cast<Enums>().ToList());
                return Ok(List);
            }
            catch
            {
                return InternalServerError();
            }
        }
        //private IHttpActionResult materialskill(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetMaterial(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult mentalskill(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetMental(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult classskill(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetClass(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult getskills(string type)
        //{
        //    var stats = xmlBase.Skills.GetSkills(type);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult healthstate(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetHealth(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult defencestate(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetDefence(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult attackstate(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetAttack(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult initiativestate(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetInitiative(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult characteristics(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetChars(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult proficiency(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetProfi(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //private IHttpActionResult info(string UserId)
        //{
        //    var stats = xmlBase.Characters.GetInfo(UserId);
        //    if (stats.Count > 0)
        //    {
        //        return Ok(stats);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
