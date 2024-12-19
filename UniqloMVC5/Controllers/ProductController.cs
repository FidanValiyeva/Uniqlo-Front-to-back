using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using UniqloMVC5.DataAccess;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModels.Basket;
using UniqloMVC5.ViewModels.Product;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Prod = UniqloMVC5.ViewModels.Product;

namespace UniqloMVC5.Controllers
{
    public class ProductController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            IQueryable<Product> query = _context.Products.Where(x => x.IsDeleted);
            ProductIndexVM vm = new ProductIndexVM

            {
                Products = await query.Select(x => new Prod.ProductItemVM
                {
                    IsInStock = x.Quantity > 0,
                    Name = x.Name,
                    ImageUrl = x.CoverImage,
                    SellPrice = x.SellPrice,
                    Id = x.Id,
                }).ToListAsync(),
                Categories = [new CategoryAndCount { Id = 0, Count = await query.CountAsync(), Name = "All" }]
            };

            var cats = await _context.Categories.Where(x => !x.IsDeleted).Select(x => new
            CategoryAndCount
            {
                Name = x.Name,
                Id = x.Id,
                Count = x.Products.Count(),


            }).ToListAsync();
            NewMethod(vm, cats);
            ViewBag.ProductCount = await query.CountAsync();
            return View(vm);

            static void NewMethod(ProductIndexVM vm, List<CategoryAndCount> cats)
            {
                vm.Categories.AddRange(cats);
            }
        }
        public async Task<IActionResult> Filter(int? catId = 0, int? minPrice = 10,int maxPrice=400)
        {
            if (!catId.HasValue)
            {
                return BadRequest();
            }
            var query = _context.Products.Where(x => !x.IsDeleted && x.SellPrice >= minPrice && x.SellPrice <= maxPrice);
            if(catId != 0 )
            {
               query=query.Where(x=>x.CategoryId==catId);
            }
            return PartialView("_ProductPartial",await query.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            return BadRequest();
            var data = await _context.Products.Where(x => x.Id == id)
                .Include(x => x.Images)
                .Include(x=>x.Ratings)
                .ThenInclude(x=>x.User)               
                .FirstOrDefaultAsync();
           

            Product vm = new Product
            {
                Name = data.Name,
                Description = data.Description,
                SellPrice = data.SellPrice,
                Discount = data.Discount,
                CoverImage = data.CoverImage,
                Images = data.Images
            };

            if (data is null) return NotFound();
            ViewBag.Rating = 5;
            if (User.Identity?.IsAuthenticated ?? false) 
            {
               string UserId = User.Claims.FirstOrDefault(x=> x.Type == ClaimTypes.NameIdentifier)?.Value;
               int rating = await _context.ProductRatings.Where(x=>x.UserId == UserId && x.ProductId==id).Select(x=>x.Rating).FirstOrDefaultAsync();    
               ViewBag.Rating = rating==0 ? 5 : rating;

            }
            
            return View(data);
        }
        public async Task<IActionResult> Rating(int productId,int rating)
        {
            string userId = User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)!.Value;
            var data=await _context.ProductRatings.Where(x=>x.UserId==userId && x.ProductId==productId).FirstOrDefaultAsync();
            if(data is null)
            {
                await _context.ProductRatings.AddAsync(new Models.ProductRating
                {
                    UserId = userId,
                    ProductId = productId,
                    Rating = rating
                });               
            }
            else
            {
             data.Rating = rating;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {Id=productId});   
        }
        public async Task<IActionResult> AddBasket(int Id)
        {
            //if(!await _context.Products.AnyAsync(x=>x.Id==Id))
            //{
            // return NotFound();  
            //}
           
            var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["Basket"] ?? "[]");

            var item = basketItems.FirstOrDefault(x => x.Id == Id);
            if(item==null)
            {
                item = new BasketProductItemVM(Id);    
                basketItems.Add(item);
            }
            else        
            item.Count++;
            Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItems));
            //object obj = new
            //{
            //    Name="ulvi",
            //    Surname="Abdullazade"

            //};

            //string a =  JsonSerializer.Serialize(obj);
            //Response.Cookies.Append()
            return Ok();
        }
       
       
    }
}
