using infludash_api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
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

        // POST api/posts/future
        [HttpPost("future")]
        [Authorize]
        public IActionResult FuturePosts()
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

                return Ok(context.posts.ToList().Where(s => s.email.Equals(email) && s.scheduled > DateTime.Now));
            }
        }

        // DELETE api/posts
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult FuturePosts(int id)
        {
            try
            {
                var post = context.posts.Where(p => p.id == id).First();
                // delete job linked with post
                var cmdTextPost = "DELETE FROM job WHERE id=@id";
                var idParam = new MySqlParameter("@id", post.postId);
                context.Database.ExecuteSqlRaw(cmdTextPost, idParam);
                context.posts.Remove(post);
                context.SaveChanges();
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
