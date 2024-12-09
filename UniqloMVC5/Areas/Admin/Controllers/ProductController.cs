using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using UniqloMVC5.DataAccess;
using UniqloMVC5.Extensions;
using UniqloMVC5.Helpers;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModels.Product;

namespace UniqloMVC5.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Product)]
    public class ProductController(IWebHostEnvironment _env, UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(x => x.Category).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {

            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.OtherFiles != null && vm.OtherFiles.Any())
            {
                if (!vm.OtherFiles.All(x => x.IsValidType("image")))
                {
                    var fileNames = vm.OtherFiles.Where(x => !x.IsValidType("image")).Select(x => x.FileName);

                    ModelState.AddModelError("OtherFiles", string.Join(",", fileNames) + "are(is)not an image");
                }
                if (!vm.OtherFiles.All(x => x.IsValidSize(300)))
                {
                    var fileNames = vm.OtherFiles.Where(x => !x.IsValidSize(3))
                        .Select(x => x.FileName);
                    ModelState.AddModelError("OtherFiles", string.Join(",", fileNames) + "must be less than 300");
                }
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View(vm);
            }

            if (vm.CoverFile != null)
            {
                if (!vm.CoverFile.IsValidType("image"))
                {
                    ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                    ModelState.AddModelError("CoverFile", "File must be an image");
                    return View(vm);
                }
                if (!vm.CoverFile.IsValidSize(300))
                {
                    ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                    ModelState.AddModelError("CoverFile", "File must be less than 300");
                    return View(vm);
                }

            }
            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.CoverFile.FileName);
            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", newFileName)))
            {
                await vm.CoverFile.CopyToAsync(stream);
            }


            Product product = new Product
            {
                CategoryId = vm.CategoryId,
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                Quantity = vm.Quantity,
                Discount = vm.Discount,
                Description = vm.Description,
                Name = vm.Name,
                CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath, "imgs", "products")


            };
            List<ProductImage> list = [];
            foreach (var item in vm.OtherFiles)
            {
                string fileName = await item.UploadAsync(_env.WebRootPath, "imgs", "products");
                list.Add(new ProductImage
                {
                    FileUrl = fileName,
                    product = product,
                });

            }
            product.Images = list;
            await _context.ProductImages.AddRangeAsync([]);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();
            var product = await _context.Products.Where(x => x.Id == id.Value)
                .Select(x => new ProductUpdateVM
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

            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            if (!id.HasValue) return BadRequest();
           
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View(vm);
            }

            if (vm.CoverFile != null)
            {
                if (!vm.CoverFile.IsValidType("image"))
                {
                    ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                    ModelState.AddModelError("CoverFile", "File must be an image");
                    return View(vm);
                }
                if (!vm.CoverFile.IsValidSize(300))
                {
                    ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                    ModelState.AddModelError("CoverFile", "File must be less than 300");
                    return View(vm);
                }

            }
            string newFileName = Path.GetRandomFileName() + Path.GetExtension(vm.CoverFile.FileName);
            using (Stream stream = System.IO.File.Create(Path.Combine(_env.WebRootPath, "imgs", "sliders", newFileName)))
            {
                await vm.CoverFile.CopyToAsync(stream);
            }
            Product product = new Product
            {
                CategoryId = vm.CategoryId,
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                Quantity = vm.Quantity,
                Discount = vm.Discount,
                Description = vm.Description,
                Name = vm.Name,
                CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath, "imgs", "products")


            };
           
            await _context.ProductImages.AddRangeAsync([]);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));






        }
    }
}
