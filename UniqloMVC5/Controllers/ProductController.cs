using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC5.DataAccess;
using UniqloMVC5.ViewModels.Product;

namespace UniqloMVC5.Controllers
{
    public class ProductController(UniqloDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            return BadRequest();                      
            var data = await _context.Products.Where(x => x.Id == id.Value && !x.IsDeleted).Select(x => new ProductVM
            {
                CategoryId = x.CategoryId,
                CostPrice = x.CostPrice,
                Quantity = x.Quantity,
                Discount = x.Discount,
                SellPrice = x.SellPrice,
                Name = x.Name,
                Description = x.Description,
                OtherFileUrls = x.Images.Select(y => y.FileUrl)
            }).FirstOrDefaultAsync();

            if(data is null) return NotFound(); 

            return View(data);
        }
    }
}
