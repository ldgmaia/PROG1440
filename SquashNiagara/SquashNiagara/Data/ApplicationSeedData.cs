﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SquashNiagara.Data
{
    public static class ApplicationSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            //Create Roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Captain", "User" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //Create Users
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            if (userManager.FindByEmailAsync("admin@squashniagara.org").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@squashniagara.org",
                    Email = "admin@squashniagara.org"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            if (userManager.FindByEmailAsync("captain@squashniagara.org").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "captain@squashniagara.org",
                    Email = "captain@squashniagara.org"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Captain").Wait();
                }
            }

            if (userManager.FindByEmailAsync("player@squashniagara.org").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "player@squashniagara.org",
                    Email = "player@squashniagara.org"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }

            //if (userManager.FindByEmailAsync("super1@outlook.com").Result == null)
            //{
            //    IdentityUser user = new IdentityUser
            //    {
            //        UserName = "super1@outlook.com",
            //        Email = "super1@outlook.com"
            //    };

            //    IdentityResult result = userManager.CreateAsync(user, "password").Result;

            //    if (result.Succeeded)
            //    {
            //        userManager.AddToRoleAsync(user, "Supervisor").Wait();
            //    }
            //}
            //if (userManager.FindByEmailAsync("user1@outlook.com").Result == null)
            //{
            //    IdentityUser user = new IdentityUser
            //    {
            //        UserName = "user1@outlook.com",
            //        Email = "user1@outlook.com"
            //    };

            //    IdentityResult result = userManager.CreateAsync(user, "password").Result;
            //Not in any role
            //}
        }
    }
}
