using Microsoft.AspNetCore.Identity;
using UniqloMVC5.Enums;
using UniqloMVC5.Models;

namespace UniqloMVC5.Extensions
{
    public static class SeedExtension
    {
        public static void UseUserSeed(this IApplicationBuilder app)
        {
            using (var scope=app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                
                if (!roleManager.Roles.Any())
                {
                    foreach(Roles item in Enum.GetValues(typeof(Roles)))
                    {
                        roleManager.CreateAsync(new IdentityRole(item.ToString())).Wait();
                    }
                }

                if (!userManager.Users.Any(x => x.NormalizedUserName == "ADMIN"))
                {

                    User user = new User
                    {
                        FullName = "admin",
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        ProfileImageUrl = "photo.jpg"

                    };

                    userManager.CreateAsync(user,"admin253admin").Wait();
                    userManager.AddToRoleAsync(user, nameof(Roles.Admin)).Wait();
                }








            }

        }
    }
}
