using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Task_3.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roles =
            {
            "Candidate",
            "Recruiter"
        };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<int>
                        {
                            Name = role
                        });
                }
            }
        }
    }

    }
