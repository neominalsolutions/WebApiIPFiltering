using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiIPFiltering.Filters;

namespace WebApiIPFiltering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("[action]")]
    
        [ServiceFilter(typeof(IpControlAttribute))]
        
        public IEnumerable<string> GetProducts()
        {
            return new List<string>
            {
             "pen",
             "mouse",
             "keyboard"
            };
        }

        [HttpGet("[action]")]
        public string GetProduct()
        {
            return "pen";
        }
    }
}
