using System.ComponentModel.DataAnnotations;

namespace UniqloMVC5.ViewModels.Auths
{
    public class LoginCreateVM
    {
        public string UsernameorEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe {  get; set; } 
        
       
        
        
    }
}
