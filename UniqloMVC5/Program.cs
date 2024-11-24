
namespace UniqloMVC5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            var app = builder.Build();
            app.UseStaticFiles();
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            
            app.Run();
        }
    }
}
