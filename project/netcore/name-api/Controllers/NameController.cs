using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace name_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NameController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            // var host=Environment.GetEnvironmentVariable("NAME_API_SERVICE_HOST");  //系统环境变量
            var host=Environment.GetEnvironmentVariable("HOSTNAME_COMMAND");  //自定义环境变量
            if (string.IsNullOrEmpty(host))
            {
                return "empty";
            }
            return host;
        }
    }
}
