﻿using infludash_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Data
{
    public class SeedDb
    {
        public static void Initialize(InfludashContext context)
        {
            context.Database.EnsureCreated();

            if (context.users.Any() || context.socials.Any())
            {
                return;
            }

            var u1 = new User() { name = "Thomas Street", email = "thomasofthestreet@outlook.com", password = "hashedpassword", passwordConfirmation = "hashedpassword", createdAt = DateTime.Now };
            context.users.Add(u1);

            var s1 = new Social() { accessToken = "specialAccessToken", SocialId = "specialId", type = SocialType.Facebook, user = u1 };
            context.socials.Add(s1);

            context.SaveChanges();
        }
    }
}