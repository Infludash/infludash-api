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
using System.Net.Http.Headers;
using System.Reflection;
using Hangfire;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace infludash_api.Controllers
{
    // api/socials
    [Route("api/[controller]")]
    [ApiController]
    public class SocialsController : Controller
    {
        private readonly InfludashContext context;
        public YoutubeService ytService = new YoutubeService();
        public SocialsController(InfludashContext infludashContext)
        {
            context = infludashContext;
        }

        // POST api/socials
        /// <summary>
        /// Returns a list of social connections
        /// </summary>
        /// <returns></returns>
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

        // GET api/socials/youtube/{email}
        /// <summary>
        /// Gets a channel by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("youtube/{email}")]
        [Authorize]
        public IActionResult Youtube(string email)
        {
            var channel = context.socials.ToList().Find(x => x.email == email) ?? null;
            if (channel == null)
            {
                return NotFound("No channel with this email found in the database.");
            }
            return Ok(channel);
        }

        // DELETE api/socials/youtube/{id}
        /// <summary>
        /// Deletes a channel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("youtube/channel/{id}")]
        [Authorize]
        public IActionResult UnlinkYtChannel(string id)
        {
            try
            {
                var social = this.context.socials.FirstOrDefault(s => s.socialId == id);
                if (social == null)
                {
                    return NotFound("No channel with this id found in the database.");
                }
                else
                {
                    this.context.socials.Remove(social);
                    try
                    {
                        this.context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                    return Ok(social.socialId);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/socials/youtube/link
        /// <summary>
        /// Links a youtube channel to your account
        /// </summary>
        /// <returns></returns>
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
                var social = new Social
                {
                    accessToken = jsonBody.socialUser.authToken,
                    socialId = jsonBody.socialUser.id,
                    type = SocialType.Youtube,
                    email = jsonBody.userEmail,
                    name = jsonBody.socialUser.name,
                    imageUrl = jsonBody.socialUser.photoUrl,
                    regionCode = jsonBody.regionCode
                };
                try
                {
                    this.context.socials.Add(social);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

                try
                {
                    this.context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                return Ok(social);
            }
        }

        // POST api/socials/youtube/channel
        /// <summary>
        /// Returns your YouTube channel
        /// </summary>
        /// <returns></returns>
        [HttpPost("youtube/channel")]
        [Authorize, ReadableBodyStream]
        public IActionResult YoutubeMe()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest();
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                string accesstoken = jsonBody.access_token;

                return Ok(JsonConvert.DeserializeObject(ytService.MyChannel(accesstoken)));
            }
        }

        // POST api/socials/youtube/upload/prepare
        /// <summary>
        /// Prepare the resumable upload
        /// </summary>
        /// <returns></returns>
        [HttpPost("youtube/upload/prepare-video")]
        [Authorize, ReadableBodyStream]
        public IActionResult YoutubeUploadPrepare()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest();
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                string accesstoken = jsonBody.access_token;
                dynamic videoData = JObject.FromObject(new
                {
                    snippet = new
                    {
                        title = jsonBody.videoData.title,
                        description = jsonBody.videoData.description,
                        tags = jsonBody.videoData.tags,
                        categoryId = jsonBody.videoData.categoryId
                    },
                    status = new
                    {
                        privacyStatus = jsonBody.videoData.privacyStatus
                    }
                });
                string videoDataString = JsonConvert.SerializeObject(videoData);
                Debug.WriteLine(videoDataString);
                string location = ytService.PrepareUploadVideo(accesstoken, videoDataString);
                Debug.WriteLine(location);
                return Ok(location == String.Empty ? "Invalid video parameter" : JsonConvert.SerializeObject(new { url = location }));
            }
        }

        // POST api/socials/youtube/upload
        /// <summary>
        /// Uploads a youtube video to your account
        /// </summary>
        /// <returns></returns>
        [HttpPost("youtube/upload/video")]
        [Authorize]
        public async Task<IActionResult> YoutubeUploadAsync()
        {
            using (var sr = new StreamReader(Request.Body))
            {
                var body = await sr.ReadToEndAsync();
                var location = Request.Headers["Location"];
                var id = Request.Headers["Id"];
                Social social;
                try
                {
                    social = context.socials.First(s => s.socialId == id.ToString());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                double scheduledFor = Double.Parse(Request.Headers["ScheduledFor"]);
                BackgroundJob.Schedule(() => ytService.UploadVideo(social.accessToken, body, location), TimeSpan.FromMinutes(scheduledFor));
                var post = new Post { email = social.email, type = SocialType.Youtube, scheduled = DateTime.Now + TimeSpan.FromMinutes(scheduledFor), title="Youtube post" };
                try
                {
                    context.posts.Add(post);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                return Ok(post);

            }
        }

        

        // POST api/socials/youtube/videoCategories
        /// <summary>
        /// Returns the available videoCategories by region
        /// </summary>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        [HttpGet("youtube/videoCategories/{regionCode}")]
        [Authorize]
        public IActionResult GetVideoCategories(string regionCode)
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                return Ok(JsonConvert.DeserializeObject(ytService.GetVideoCategories(regionCode)));
            }
        }
        #endregion

        #region Facebook
        // POST api/socials/facebook/link
        /// <summary>
        /// Links a youtube channel to your account
        /// </summary>
        /// <returns></returns>
        [HttpPost("facebook/link")]
        [Authorize, ReadableBodyStream]
        public IActionResult LinkFacebook()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest();
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                var social = new Social
                {
                    accessToken = jsonBody.socialUser.authToken,
                    socialId = jsonBody.socialUser.id,
                    type = SocialType.Facebook,
                    email = jsonBody.userEmail,
                    name = jsonBody.socialUser.name,
                    imageUrl = jsonBody.socialUser.photoUrl,
                    regionCode = jsonBody.regionCode
                };
                try
                {
                    this.context.socials.Add(social);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

                try
                {
                    this.context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                return Ok(social);
            }
        }

        // POST api/socials/facebook/post
        [HttpPost("facebook/post")]
        [Authorize, ReadableBodyStream]
        public IActionResult FbPost()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest();
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                
               
                return Ok();
            }
        }
        #endregion
    }
}
