﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeDevCode.Database
{
    public class AppSeeder
    {
        public static async Task InitializeAsync(AppDatabase database, UserManager<User> user, RoleManager<IdentityRole> role)
        {
            database.Database.EnsureCreated();

            if (!role.Roles.Any())
            {
                await role.CreateAsync(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"

                }) ;

                await role.CreateAsync(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER"

                });
            }

            if (!user.Users.Any())
            {
                var createAdminResult = await user.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Email = "admin@email.com",
                    LockoutEnabled = false
                },
                "Admin@123"
                );

                if (createAdminResult.Succeeded)
                {
                    var userResult = await user.FindByNameAsync("admin");
                    await user.AddToRoleAsync(userResult, "Admin");
                }
                var createUserResult = await user.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "user",
                    Email = "user@email.com",
                    LockoutEnabled = false
                },
                "User@123"
                );

                if (createAdminResult.Succeeded)
                {
                    var userResult = await user.FindByNameAsync("user");
                    await user.AddToRoleAsync(userResult, "User");
                }
            }
        }
    }
}
