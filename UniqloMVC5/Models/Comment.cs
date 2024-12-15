namespace UniqloMVC5.Models
{
    public class Comment:BaseEntity
    {
        public string Username {  get; set; }
        public int Rating { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }        
        public ICollection<Product> Products { get; set; }
    }
}
