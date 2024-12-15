
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UniqloMVC5.DataAccess;
using UniqloMVC5.Extensions;
using UniqloMVC5.Helpers;
using UniqloMVC5.Models;
using UniqloMVC5.Services.Abstracts;
using UniqloMVC5.Services.Implements;

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
               
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDbContext>();
            builder.Services.ConfigureApplicationCookie(x =>
            {
                x.AccessDeniedPath = "/Home/AccessDenied";
               
            });

            builder.Services.AddScoped<IEmailService, EmailService>();
            var opt = new SmtpOptions();
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

            //builder.Services.AddSession();

            var app = builder.Build();
            app.UseStaticFiles();

            app.UseUserSeed();
            //app.UseSession();
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
