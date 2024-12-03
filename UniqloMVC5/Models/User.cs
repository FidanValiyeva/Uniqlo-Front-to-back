using Microsoft.AspNetCore.Identity;

namespace UniqloMVC5.Models
{
    public class User:IdentityUser
    {
      public string FullName {  get; set; }
      public string ProfileImageUrl {  get; set; }


    }
}
