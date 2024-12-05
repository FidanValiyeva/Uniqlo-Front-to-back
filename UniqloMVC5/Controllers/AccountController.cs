using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModels.Auths;

namespace UniqloMVC5.Controllers
{
    public class AccountController(UserManager<User> _userManager,SignInManager<User> signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM vm)
        {
            if (ModelState.IsValid)
            {              
                return View();
            }
               


            User user =new User
            {
                Email = vm.Email,
                FullName = vm.FullName,
                UserName = vm.UserName,
                ProfileImageUrl="photo.jpg"
               
            };
           var result = await _userManager.CreateAsync(user,vm.Password);
            if (result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(result);
            }
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]  
        public  async Task<IActionResult>Login(LoginCreateVM vm)
        {

            if (ModelState.IsValid)
            {return View();}
            User? user = null;
            if (vm.UsernameorEmail.Contains('@'))
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameorEmail);
            }
            else
            { 
                user = await _userManager.FindByNameAsync(vm.UsernameorEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
            }
            //await _signInManager.Password
           
            return View();
        }
            
    }
}
