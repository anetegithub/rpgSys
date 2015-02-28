using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace rpgSys.Controllers
{
    public class ScenarioController : ApiController
    {
        [ActionName("create")]
        public string Post([FromBody]string value)
        {
            Scenario Scenario=new JavaScriptSerializer().Deserialize<Scenario>(value);
            //xmlBase.Scenarios.New = Scenario;
            return "Сценарий отправлен на рассмотрение.\nЕсли на сервере включена ручная проверка сценариев, тогда сценарий будет доступен только после проверки.";
        }
    }
}
