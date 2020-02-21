using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s_demo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace k8s_demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly INameService _nameService;
        public ValuesController(INameService nameService)
        {
            _nameService=nameService;
        }

        // GET: api/Values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            return "hello,"+await _nameService.GetName();
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
