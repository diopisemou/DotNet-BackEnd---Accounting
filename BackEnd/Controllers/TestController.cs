using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BackEnd.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get () {
            return new string[] { "test1", "test2" };
        }

        [HttpGet("pp")]
        public ActionResult<IEnumerable<string>> Get (string id) {
            return new string[] { "test1", id };
        }
        [HttpPost("add")]
        public ActionResult<IEnumerable<string>> Post ([FromBody] JObject value) {
            Console.Write("here");
            return new string[] { value["value"].Value<string>() };
        }


    }
}