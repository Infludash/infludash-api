using infludash_api.Data;
using infludash_api.Models;
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
    // api/preferences
    [Route("api/preferences")]
    [ApiController]
    public class PreferencesController : Controller
    {
        private readonly InfludashContext context;

        public PreferencesController(InfludashContext infludashContext)
        {
            context = infludashContext;
        }

        // POST api/preferences/youtube
        [HttpPost("youtube")]
        [Authorize]
        public IActionResult YtPreferences()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest("You must specify the user email");
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                string email = jsonBody.email;

                return Ok(context.yt_preferences.ToList().Where(s => s.email.Equals(email)));
            }
        }
        
        // POST api/preferences/youtube/description
        [HttpPost("youtube/description")]
        [Authorize]
        public IActionResult AddDescriptionYtPreferences()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest("You must specify the user email");
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                string email = jsonBody.email;
                string description = jsonBody.description;

                if (!context.yt_preferences.ToList().Exists(s => s.email.Equals(email)))
                {
                    YtPreference ytp = new YtPreference{ email=email, description=description };
                    try
                    {
                        context.yt_preferences.Add(ytp);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }
                else
                {
                    YtPreference ytp = context.yt_preferences.First(s => s.email.Equals(email));
                    ytp.description = description;

                    context.Update(ytp);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

                return Ok();
            }
        }
        
        // POST api/preferences/youtube/tags
        [HttpPost("youtube/tags")]
        [Authorize]
        public IActionResult AddCategoriesYtPreferences()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest("You must specify the user email");
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                string email = jsonBody.email;
                string[] tags = jsonBody.tags.ToObject<string[]>();

                if (!context.yt_preferences.ToList().Exists(s => s.email.Equals(email)))
                {
                    YtPreference ytp = new YtPreference{ email=email, tags=string.Join(",", tags) };
                    try
                    {
                        context.yt_preferences.Add(ytp);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }
                else
                {
                    YtPreference ytp = context.yt_preferences.First(s => s.email.Equals(email));
                    if (tags.Length > 0)
                    {
                        ytp.tags = string.Join(",", tags);
                    }
                    else
                    {
                        ytp.tags = null;
                    }

                    context.Update(ytp);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

                return Ok();
            }
        }

        // POST api/preferences/youtube/categories
        [HttpPost("youtube/categories")]
        [Authorize]
        public IActionResult AddTagsYtPreferences()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                Task<string> body = stream.ReadToEndAsync();
                if (body.Result == "")
                {
                    return BadRequest("You must specify the user email");
                }
                dynamic jsonBody = JObject.Parse(body.Result);
                string email = jsonBody.email;
                string[] categories = jsonBody.categories.ToObject<string[]>();

                if (!context.yt_preferences.ToList().Exists(s => s.email.Equals(email)))
                {
                    YtPreference ytp = new YtPreference { email = email, categories = string.Join(",", categories) };
                    try
                    {
                        context.yt_preferences.Add(ytp);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }
                else
                {
                    YtPreference ytp = context.yt_preferences.First(s => s.email.Equals(email));
                    if (categories.Length > 0)
                    {
                        ytp.categories = string.Join(",", categories);
                    }
                    else
                    {
                        ytp.categories = null;
                    }

                    context.Update(ytp);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

                return Ok();
            }
        }
    }
}
