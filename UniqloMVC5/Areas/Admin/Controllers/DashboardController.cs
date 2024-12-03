using Microsoft.AspNetCore.Mvc;

namespace UniqloMVC5.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
