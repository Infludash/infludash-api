using infludash_api.Data;
using infludash_api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Controllers
{
    // api/users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private InfludashContext myDbContext;

        public UsersController(InfludashContext context)
        {
            myDbContext = context;
        }

        // GET api/users
        [HttpGet]
        public IList<User> GetAll()
        {
            return (this.myDbContext.users.ToList());
        }

        // POST: api/users
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            

            return Created("", user);
        }
    }
}
