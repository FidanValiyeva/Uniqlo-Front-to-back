using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using UniqloMVC5.DataAccess;
using UniqloMVC5.Models;
using UniqloMVC5.ViewModels.Basket;
using Prod = UniqloMVC5.ViewModels.Product;

namespace UniqloMVC5.Controllers
{
    public class ProductController(UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {

           return View(await _context.Products.Where(x => !x.IsDeleted).Select(x => new 
           Prod.ProductItemVM
            {
             IsInStock=x.Quantity > 0,            
             Name=x.Name,
             ImageUrl=x.CoverImage,
             SellPrice=x.SellPrice,
             Id=x.Id,


            }).ToListAsync());
           
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
