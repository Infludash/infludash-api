using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Controllers
{
    // GET api/socials
    [Route("api/[controller]")]
    [ApiController]
    public class SocialsController : Controller
    {
        // GET api/socials/test
        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("authorized path");
        }
    }
}
