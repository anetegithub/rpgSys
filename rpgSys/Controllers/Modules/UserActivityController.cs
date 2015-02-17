using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace rpgSys.Controllers
{
    public class UserActivityController : ApiController
    {
        public IHttpActionResult Get(string UserId)
        {
            if (UserId != "undefined")
            {
                var Data = xmlBase.Modules.UserActivityes.Get(UserId)[0];
                if (Data != null)
                {
                    foreach (var a in Data.Activityes)
                    {
                        a.Stamp = TimeAgo(Convert.ToDateTime(a.Stamp));
                    }

                    return Ok(Data);
                }
                else
                {
                    return BadRequest("User is not found!");
                }
            }
            else
            { return NotFound(); }
        }

        //Переписано на реалии ru-RU
        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return Format(years, new string[] { "год", "года", "лет" });
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return Format(months, new string[] { "месяц", "месяца", "месяцев" });
            }
            if (span.Days > 0)
                return Format(span.Days, new string[] { "день", "дня", "дней" });
            if (span.Hours > 0)
                return Format(span.Hours, new string[] { "час", "часа", "часов" });
            if (span.Minutes > 0)
                return Format(span.Minutes, new string[] { "минута", "минуты", "минут" });
            if (span.Seconds > 5)
                return String.Format("{0} секунд назад", span.Seconds);
            if (span.Seconds <= 5)
                return "Только что";
            return string.Empty;
        }

        public static string Format(int Number, string[] variants)
        {
            string retrn = Number.ToString() + " ";
            int numb = Convert.ToInt32(Number.ToString()[Number.ToString().Length - 1].ToString());
            if (numb == 0)
                retrn+=variants[2];
            else if (numb == 1)
                retrn += variants[0];
            else if (numb < 5)
                retrn += variants[1];
            else
                retrn += variants[2];
            return retrn += " назад";
        }
    }
}
