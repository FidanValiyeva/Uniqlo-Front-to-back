using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModels.Auths;

namespace UniqloMVC5.Controllers
{
    public class AccountController(UserManager<User> userManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM vm)
        {
            if (ModelState.IsValid) 
                return View();
            User user =new User();
            {
                

            }
            return View();
        }
    }
}
