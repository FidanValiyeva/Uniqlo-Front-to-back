using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC5.DataAccess;
using UniqloMVC5.ViewModels.Category;
using UniqloMVC5.ViewModels.Slider;

namespace UniqloMVC5.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
           var datas = await _context.Categories.Select(x => new CategoryItemVM
            {
                Id = x.Id,
                Name = x.Name,
                Url = x.CoverImageUrl,
            }
              ).ToListAsync();
            return View(datas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if(vm.File != null)
            {
                if (!vm.File.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("File", "File Must be an image");
                    return View();
                }
                if (vm.File.Length > 6 * 1024)
                {
                    ModelState.AddModelError("File", "File must be bigger than 6kb");
                    return View();
                }
                if (!ModelState.IsValid)
                {
                    return View();
                }
                string fileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
                string path= Path.Combine(_env.WebRootPath,"imgs","categories", fileName);

                using(Stream sr =System.IO.File.Create(path))
                {
                    await vm.File.CopyToAsync(sr);
                }
                await _context.Categories.AddAsync(new Models.Category
                {
                    Name = vm.Name,
                    CoverImageUrl = fileName,
                    Description = "asd"
                });
                await _context.SaveChangesAsync();               
            }
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var category = await _context.Categories.Where(x => x.Id == id.Value)
                .Select(x => new CategoryUpdateVM
                {
                 Name= x.Name,                
                   
                }).FirstOrDefaultAsync();

            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View(category);
        }
        public async Task<IActionResult> Update(int? id , CategoryCreateVM vm)
        {

            if (vm.File != null)
            {
                if (!vm.File.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("File", "File Must be an image");
                    return View();
                }
                if (vm.File.Length > 6 * 1024)
                {
                    ModelState.AddModelError("File", "File must be bigger than 6kb");
                    return View();
                }
                if (!ModelState.IsValid)
                {
                    return View();
                }
                string fileName = Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
                string path = Path.Combine(_env.WebRootPath, "imgs", "categories", fileName);

                using (Stream sr = System.IO.File.Create(path))
                {
                    await vm.File.CopyToAsync(sr);
                }
                await _context.Categories.AddAsync(new Models.Category
                {
                    Name = vm.Name,
                    CoverImageUrl = fileName,
                    Description = "asd"
                });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if(id.HasValue)
            return BadRequest();            
            var category=await _context.Categories
                .Where(c=> c.Id == id.Value)
                .FirstOrDefaultAsync();
            if(category == null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "imgs", "categories", category.CoverImageUrl);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            if (Path.Exists(path))
            { 
                System.IO.File.Delete(path);
            }
            return RedirectToAction(nameof(Index));
            
        }



    }
}
