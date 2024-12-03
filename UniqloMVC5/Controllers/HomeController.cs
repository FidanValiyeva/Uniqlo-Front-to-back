using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniqloMVC5.DataAccess;
using UniqloMVC5.ViewModels.Category;
using UniqloMVC5.ViewModels.Common;
using UniqloMVC5.ViewModels.Product;
using UniqloMVC5.ViewModels.Slider;

namespace UniqloMVC5.Controllers
{
    public class HomeController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM();
            vm.Sliders = await _context.Sliders            
                .Where(x => !x.IsDeleted)
                .Select(x => new SliderItemVM
                {

                ImageUrl=x.ImageUrl,
                Link=x.Link,
                Title=x.Title,
                Subtitle=x.Subtitle

                }).ToListAsync();
            
            vm.Products = await _context.Products          
                .Where(x => !x.IsDeleted)
                .Select(x => new ProductItemVM
                {

                    Id=x.Id,
                    Name = x.Name,
                    Description = x.Description,
                   IsInStock=x.Quantity > 0,                                      
                }).ToListAsync();

            vm.Categories = await _context.Categories
               .Where(x => !x.IsDeleted)
               .Select(x => new CategoryItemVM
               {
                   Url=x.CoverImageUrl,
                   Id = x.Id,
                   Name = x.Name,
                   
               }).ToListAsync();


            return View(vm);
        }        
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
       


    }
}
