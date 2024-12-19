namespace UniqloMVC5.ViewModels.Product
{
    public class ProductIndexVM
    {
        public int ProductCount { get; set; }
        public  IEnumerable<ProductItemVM> Products{ get; set;}
        public List<CategoryAndCount> Categories{ get; set;}
    }
    public class CategoryAndCount
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public int Count {  get; set; } 

    }

}
