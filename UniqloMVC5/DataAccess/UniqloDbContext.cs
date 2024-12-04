using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniqloMVC5.Models;

namespace UniqloMVC5.DataAccess
{
    public class UniqloDbContext:IdentityDbContext<User>
    {
        public DbSet<Slider>Sliders { get; set;}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public UniqloDbContext(DbContextOptions opt):base(opt)
        { }
       
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<ProductImage>(
                x=>x.Property(y=>y.CreatedTime)
                .HasDefaultValueSql("GETDATE()"));
            base.OnModelCreating(modelBuilder);
            }

    }                    
        
    
}

