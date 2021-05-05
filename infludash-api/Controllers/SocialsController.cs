using infludash_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using infludash_api.Attributes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using infludash_api.Data;
using infludash_api.Models;

namespace infludash_api.Controllers
{
    // api/socials
    [Route("api/[controller]")]
    [ApiController]
    public class SocialsController : Controller
    {
        YoutubeService ytService = new YoutubeService();
        private readonly InfludashContext context;
        public SocialsController(InfludashContext infludashContext)
        {
            context = infludashContext;
        }

        // POST api/socials
        [HttpPost]
        [Authorize, ReadableBodyStream]
        public IActionResult Socials()
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
                
                return Ok(context.socials.ToList().Where(s => s.email.Equals(email)));
            }
        }

        // api/socials/youtube
        #region Youtube
        // GET api/socials/youtube/me
        [HttpPost("youtube/link")]
        [Authorize, ReadableBodyStream]
        public IActionResult LinkYoutube()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest();
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                var social = new Social { 
                    accessToken = jsonBody.socialUser.response.access_token, 
                    socialId = jsonBody.socialUser.id, 
                    type = SocialType.Youtube,
                    email = jsonBody.userEmail,
                    name = jsonBody.socialUser.name
                };

                if (context.socials.ToList().Contains(social))
                {
                    return BadRequest("This channel is already linked to this account");
                }
                this.context.socials.Add(social);
                try
                {
                    this.context.SaveChanges();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
                return Ok(social);
            }
        }

        // GET api/socials/youtube/me
        [HttpGet("youtube/me")]
        [Authorize]
        public IActionResult YoutubeMe()
        {
            ytService.MyChannel("");
            return Ok("authorized path");
        }
        #endregion
    }
}
