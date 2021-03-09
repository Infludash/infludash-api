using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Controllers
{
    // api/socials
    [Route("api/[controller]")]
    [ApiController]
    public class SocialsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
