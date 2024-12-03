using System.ComponentModel.DataAnnotations;

namespace UniqloMVC5.ViewModel.Slider
{
    public class SliderCreateVM
    {
        [MaxLength (32,ErrorMessage= "Title Length must be less than 32"),Required(ErrorMessage="Title is required")]
        public string Title { get; set; }
        [MaxLength(64),Required]
        public string Subtitle {  get; set; }
        public string? Link { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
