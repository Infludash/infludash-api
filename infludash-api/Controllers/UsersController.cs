using infludash_api.Data;
using infludash_api.Services;
using infludash_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace infludash_api.Controllers
{
    // api/users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private InfludashContext myDbContext;
        private IConfiguration _config;

        public UsersController(InfludashContext context, IConfiguration config)
        {
            _config = config;
            myDbContext = context;
        }

        // GET api/users
        [HttpGet]
        [Authorize]
        public IList<User> GetAll()
        {
            return (this.myDbContext.users.ToList());
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            user.createdAt = DateTime.Now;
            user.password = BC.HashPassword(user.password);
            myDbContext.users.Add(user);
            await myDbContext.SaveChangesAsync();

            return Created("", user);
        }

        // POST api/users/authenticate
        [HttpPost("authenticate")]
        public IActionResult Authenticate(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            // get account from database
            var account = myDbContext.users.SingleOrDefault(x => x.email == user.email);

            // check account found and verify password
            if (account == null || !BC.Verify(user.password, account.password))
            {
                // authentication failed
                return Unauthorized("Credentials do not match our records");
            }
            else
            {
                // authentication successful
                var jwt = new JwtService(_config);
                var token = jwt.GenerateSecurityToken(user.email);
                return Ok(token);
            }
        }
    }
}
