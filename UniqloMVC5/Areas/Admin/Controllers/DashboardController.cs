using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniqloMVC5.Enums;

namespace UniqloMVC5.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =nameof(Roles.Admin))]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
