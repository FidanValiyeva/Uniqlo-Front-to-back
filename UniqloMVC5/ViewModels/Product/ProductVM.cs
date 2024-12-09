using System.ComponentModel.DataAnnotations;

namespace UniqloMVC5.ViewModels.Product
{
    public class ProductVM
    {
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        public string? Description { get; set; } = null!;
        public decimal? CostPrice { get; set; }
        public decimal? SellPrice { get; set; }
        public int? Quantity { get; set; }
        public int Discount { get; set; }
        public IFormFile CoverFile { get; set; } = null!;
        public IEnumerable<string> OtherFileUrls { get; set; }
        public IEnumerable<IFormFile> OtherFiles { get; set; }
        public int CategoryId { get; set; }
    }
}
