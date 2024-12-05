using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using UniqloMVC5.DataAccess;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModel.Slider;
using UniqloMVC5.ViewModels.Product;
using UniqloMVC5.ViewModels.Slider;

namespace UniqloMVC5.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {

        //readonly uniqlodbcontext _context;
        //public slidercontroller(uniqlodbcontext context, ıwebhostenvironment env)
        //{
        //    _context = context;
        //}

        public async Task<IActionResult> Index()
        {
           
            return View(await _context.Sliders.ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (SliderCreateVM vm)
        { 
            if (!ModelState.IsValid)
            {
            return View(vm);
            }
            if(!vm.File.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("File", "File type must be image");
                return View(vm);
            }

            if(vm.File.Length > 6*1024)
            {
                ModelState.AddModelError("File", "File Length must be least than 6kb");
                return View(vm);    
            }
            
            string newFileName=Path.GetRandomFileName() + Path.GetExtension(vm.File.FileName);
            using(Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath,"imgs","sliders",newFileName)))
            {
                await vm.File.CopyToAsync(stream);
            }
            Slider slider = new Slider
            {
                ImageUrl = newFileName,
                Link = vm.Link,
                Subtitle = vm.Subtitle,
                Title=vm.Title,
            };
            

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

           return RedirectToAction(nameof(Index));
            
           
        }
       
        [HttpPost]
        
          public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var slider = await _context.Sliders.Where(x => x.Id == id.Value)
                .Select(x => new SliderUpdateVM
                {
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    ImageUrl = x.ImageUrl,
                    Link = x.Link

                }).FirstOrDefaultAsync();

            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View(slider);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            if (!id.HasValue) return BadRequest();
            if (vm.File != null)
            {
                if (!vm.File.ContentType.StartsWith("image"))
                {
                    ModelState.AddModelError("File", "File type must be an image");
                }
                if (vm.File.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "File size must be less than 5mb");
                }
            }
            if (!ModelState.IsValid) return View(vm);
            var sliders = await _context.Sliders
                .Where(c => c.Id == id.Value)
                .FirstOrDefaultAsync();
            if (sliders is null) return NotFound();
            if (vm.File != null)
            {
                string path = Path.Combine(_env.WebRootPath, "imgs", "sliders", sliders.ImageUrl);
                using (Stream sr = System.IO.File.Create(path))
                {
                    await vm.File!.CopyToAsync(sr);
                }
            }
            sliders.Title = vm.Title;
            sliders.Subtitle = vm.Subtitle;
            sliders.Link = vm.Link;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }




    
}
