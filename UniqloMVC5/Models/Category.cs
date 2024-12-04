using System.ComponentModel.DataAnnotations;

namespace UniqloMVC5.Models
{
    public class Category:BaseEntity
    {

        public string Name { get; set; }        
        public string CoverImageUrl {  get; set; }    
        public string   Description { get; set; }
        

    }
}
