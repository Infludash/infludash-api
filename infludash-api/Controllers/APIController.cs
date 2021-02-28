using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Controllers
{
    [ApiController]
    public class APIController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
