using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using UniqloMVC5.Enums;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModels.Auths;

namespace UniqloMVC5.Controllers
{
    public class AccountController(UserManager<User> _userManager,SignInManager<User> _signInManager) : Controller
    {
        bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;
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

            var roleResult = await _userManager.AddToRoleAsync(user,nameof(Roles.User));

            if (!roleResult.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
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
        public  async Task<IActionResult>Login(LoginCreateVM vm,string? returnUrl = null)
        {
            if (isAuthenticated)
            { return RedirectToAction("Index", "Home"); }

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
                return View();  
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password,vm.RememberMe,true);

            if(!result.Succeeded)
            {
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "username or password Wrong");
                    return View();

                }
                if (result.IsLockedOut) 
                {
                    ModelState.AddModelError("", "wait until" + user.LockoutEnd.Value.ToString("yyyy - MM - dd HH:mm:ss"));
                    return View();
                }              

            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                if(await _userManager.IsInRoleAsync(user,"Admin"))
                {
                    return RedirectToAction("Index", new {Controller="Dashboard",Area="Admin"});
                }
                else
                return RedirectToAction("Index", "Home");
            }
            else
            return LocalRedirect(returnUrl);
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login)); 
        }
        public async Task<IActionResult> Test()
        {
            SmtpClient smtp = new();
            smtp.Host = "localhost";
            return View();
        }
            
    }
}
