using infludash_api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Controllers
{
    // api/socials
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly InfludashContext context;
        public PostsController(InfludashContext infludashContext)
        {
            context = infludashContext;
        }

        // POST api/posts
        [HttpPost]
        [Authorize]
        public IActionResult Posts()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest("You must specify the user email");
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                string email = jsonBody.userEmail;

                return Ok(context.posts.ToList().Where(s => s.email.Equals(email)));
            }
        }
    }
}
