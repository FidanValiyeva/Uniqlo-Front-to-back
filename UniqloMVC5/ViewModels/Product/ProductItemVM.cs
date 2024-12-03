using System.ComponentModel.DataAnnotations;

namespace UniqloMVC5.ViewModels.Product
{
    public class ProductItemVM
    {
       public int Id { get; set; }  
       public string Name { get; set; }
       public string Description { get; set; }
        public decimal SellPrice {  get; set; }
        public int DisCount {  get; set; }
        public bool IsInStock {  get; set; }
        public string ImageUrl {  get; set; }

    }
}
