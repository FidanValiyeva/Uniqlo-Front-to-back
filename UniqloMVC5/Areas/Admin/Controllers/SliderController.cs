﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using UniqloMVC5.DataAccess;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModel.Slider;

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
        public IActionResult Create()
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


    }
}