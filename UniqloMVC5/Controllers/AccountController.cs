using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using UniqloMVC5.Enums;
using UniqloMVC5.Helpers;
using UniqloMVC5.Models;
using UniqloMVC5.Services.Abstracts;
using UniqloMVC5.ViewModels.Auths;

namespace UniqloMVC5.Controllers
{
    public class AccountController(UserManager<User> _userManager,SignInManager<User> _signInManager,IOptions<SmtpOptions>opts,IEmailService _service) : Controller
    {
       readonly SmtpOptions _smtpOpt=opts.Value;
        bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM vm)
        {
            
            if (!ModelState.IsValid)
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
            if (!result.Succeeded)
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
            string token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _service.SendEmailConfirmation(user.Email,user.UserName,token);
            return Content ("Email sent ");
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

            if (!ModelState.IsValid)
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
        //public async Task<IActionResult> Test()
        //{
        //    SmtpClient smtp = new();
        //    smtp.Host = smtp.Host;
        //    smtp.Port = smtp.Port;
        //    smtp.EnableSsl = true;
        //    smtp.Credentials = new NetworkCredential(_smtpOpt.Username, _smtpOpt.Password );
        //    MailAddress From = new MailAddress(_smtpOpt.Username, "Səfərbərlik və Hərbi Xidmətə Çağırış üzrə Dövlət Xidməti");
        //    MailAddress to = new("kxveliyeva@gmail.com");
        //    MailMessage message = new MailMessage(From,to);
        //    message.Subject = "Təqaüdün dayandırılması barədə bildiriş";
        //    message.Body = "Hörmətli Könül Veliyeva,Azərbaycan Respublikası Təhsil Nazirliyinin Təqaüd Proqramları Departamenti tərəfindən aparılmış araşdırmalar nəticəsində məlum olmuşdur ki, sizin tələbə dosyenizdə bəzi uyğunsuzluqlar qeydə alınmışdır." +
        //        "Bu səbəbdən təqaüd ödənişləriniz 2024-cü il dekabr ayının 16-sı tarixindən etibarən dayandırılmışdır. Müvafiq sənədləri təqdim edərək bu qərarın yenidən baxılması üçün +994508686112 ilə əlaqə saxlamağınız xahiş olunur." +
        //        "Təqaüdün bərpası ilə bağlı müraciətlər 30-u tarixinədək qəbul edilir. Göstərilən tarixədək lazımi sənədlər təqdim olunmadığı halda, qərar qüvvədə qalacaq.Hörmətlə,Azərbaycan Respublikası Təhsil NazirliyiTəqaüd Proqramları Departament";
        //    message.IsBodyHtml=true; 
        //    smtp.Send(message);
        //    return Ok("oldu");

        //}
        public async Task<IActionResult> VerifyEmail(string token,string user)
        {
            var entity =await _userManager.FindByNameAsync(user);
            if (entity == null)
            {
                return BadRequest();
            }
           var result= await _userManager.ConfirmEmailAsync(entity,token.Replace(' ','+'));
            if (!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder(); 
                foreach (var item in result.Errors)
                { 
                sb.AppendLine(item.Description); 
                }
                return Content(sb.ToString());
            }
            await _signInManager.SignInAsync(entity, true);
            return RedirectToAction("Index","Home");
        }

    }
}
