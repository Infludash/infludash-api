﻿using infludash_api.Data;
using infludash_api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace infludash_api.Controllers
{
    // api/user
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly InfludashContext context;
        public UserController(InfludashContext infludashContext)
        {
            context = infludashContext;
        }
        // Delete account (everything)
        [HttpDelete("{email}"), Authorize]
        public IActionResult DeleteAccount(string email)
        {
            try
            {
                // delete account_emailaddress
                var cmdText = "DELETE FROM account_emailaddress WHERE email=@email";
                var emailParam = new MySqlParameter("@email", email);
                context.Database.ExecuteSqlRaw(cmdText, emailParam);

                // delete auth_user
                var cmdText2 = "DELETE FROM auth_user WHERE email=@email";
                context.Database.ExecuteSqlRaw(cmdText2, emailParam);

                // delete socials
                var socials = context.socials.Where(p => p.email == email).ToList();
                foreach (var social in socials)
                {
                    context.socials.Remove(social);
                }

                var posts = context.posts.Where(p => p.email == email).ToList();

                // delete jobs
                foreach (var post in posts)
                {
                    var cmdTextPost = "DELETE FROM job WHERE id=@id";
                    var idParam = new MySqlParameter("@id", post.postId);
                    context.Database.ExecuteSqlRaw(cmdTextPost, idParam);
                }

                // delete posts
                foreach (var post in posts)
                {
                    context.posts.Remove(post);
                }

                return Ok(context.SaveChanges());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Return account (everything)
        [HttpGet("{email}"), Authorize]
        public IActionResult GetAllAccountData(string email)
        {
            List<(string, string)> headers = new List<(string, string)>();
            headers.Add(("Authorization", Request.Headers["Authorization"]));
            // get account
            var user = Helper.HttpGetRequest("http://127.0.0.1:8000/auth/user/", headers);
            // get linked socials
            var socials = context.socials.Where(s => s.email == email).ToList();
            // get posts
            var posts = context.posts.Where(p => p.email == email).ToList();

            var response = new
            {
                account = JsonConvert.DeserializeObject(user),
                socials = socials,
                posts = posts
            };

            return Ok(response);
        }
    }
}
