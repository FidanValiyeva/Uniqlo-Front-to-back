using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace UniqloMVC5.Controllers
{
    public class TestController : Controller
    {
        public IActionResult SetSession(string key,string value)
        {
            HttpContext.Session.SetString(key,value);
          return View();
        }
        public async Task<IActionResult>GetSession(string key)
        {
           HttpContext.Session.Remove(key);
           HttpContext.Session.Clear();
           return Content(HttpContext.Session.GetString(key));
        }
        public async Task<IActionResult> SetCookie(string key,string value)
        {
            HttpContext.Response.Cookies.Append(key,value,new CookieOptions 
            {
                //Expires= new DateTime(2024,12,31,00,00,00)
                MaxAge=TimeSpan.FromMinutes(1)

            });
            return Ok();
        }
        public async Task<IActionResult> GetCookie(string key)
        {
            
            return Content(HttpContext.Request.Cookies[key]);
        }

    }
}
