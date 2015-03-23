using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ormCL;

namespace rpgSys
{
    public class ActivityController : ApiController
    {
        public IHttpActionResult Get()
        {
            try
            {
                var Collection = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/User/Activity") }).Cast<UserActivity>().ToList();
                foreach(var Item in Collection)
                {
                    //Item.StampToString = Item.Stamp.Ago();
                }
                return Ok(Collection);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Get(int Id)
        {
            try
            {
                var Collection = new baseCL("Data").Select(new requestCL() { Table = new tableCl("/User/Activity") })
                    .Cast<UserActivity>()
                    .Filter(new conditionCL("UserId.==."+Id.ToString()))
                    .Sort(new sortingCL("Id:Desc"))
                    .Limit(10)
                    .ToList();
                foreach (var Item in Collection)
                {
                    //Item.StampToString = Item.Stamp.Ago();
                }
                return Ok(Collection);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ActionName("add")]
        public bool Put([FromBody] string Settings)
        {
            return false;
        }
    }
}