using Entities.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Online_Quize_System.Seed_Data
{
    public static class SeedData
    {

        public static void SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<QuizContext>())
            {
                context.Database.EnsureCreated();

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roleNames = { "Admin", "Student" };
                foreach (var roleName in roleNames)
                {
                    if (!roleManager.RoleExistsAsync(roleName).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                    }
                }

                var adminUser = context.Users.FirstOrDefault(u => u.UserName == "admin@example.com");
                if (adminUser == null)
                {
                    var newAdmin = new ApplicationUser
                    {   Name="admin",
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(newAdmin, "Admin@123").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(newAdmin, "Admin").Wait();
                    }
                }

                var studentUser = context.Users.FirstOrDefault(u => u.UserName == "student@example.com");
                if (studentUser == null)
                {
                    var newStudent = new ApplicationUser
                    {   Name="student",
                        UserName = "student@example.com",
                        Email = "student@example.com",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(newStudent, "Student@123").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(newStudent, "Student").Wait();
                    }
                }

                context.SaveChanges();
            }
        }


    }
}
