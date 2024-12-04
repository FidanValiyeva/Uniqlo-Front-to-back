
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UniqloMVC5.DataAccess;
using UniqloMVC5.Models;

namespace UniqloMVC5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<UniqloDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"));
            });

            //builder.Services.AddScoped<UniqloDbContext>();
            builder.Services.AddControllersWithViews();



            builder.Services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDbContext>();



            var app = builder.Build();
            app.UseStaticFiles();

            app.MapControllerRoute(name: "register",
                pattern: "register",
                defaults: new { controller = "Account", action = "Register" });


            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name:"default",
                pattern:"{controller=Home}/{action=Index}/{id?}");
            
            app.Run();
        }
    }
}
