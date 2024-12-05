using System.ComponentModel.DataAnnotations;

namespace UniqloMVC5.ViewModels.Auths
{
    public class UserCreateVM
    {
        [Required,MaxLength(20)]
        public string FullName {  get; set; }
        [Required,MaxLength(64)]
        public string UserName { get; set; }
        [Required,MaxLength(64),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,MaxLength(128),EmailAddress]
        public string? Email { get; set; }
        [Required, MaxLength(64), DataType(DataType.Password),Compare(nameof(Password))]
        public string RePassword {  get; set; }

    }
}
