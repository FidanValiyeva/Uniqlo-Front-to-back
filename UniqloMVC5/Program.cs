
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UniqloMVC5.DataAccess;

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
            
            var app = builder.Build();
            app.UseStaticFiles();


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
