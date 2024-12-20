﻿
using System.ComponentModel.DataAnnotations;

namespace UniqloMVC5.ViewModels.Slider
{
    public class SliderUpdateVM
    {
        [MaxLength(32, ErrorMessage = "Title Length must be less than 32"), Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [MaxLength(64), Required]
        public string Subtitle { get; set; }
        public string? Link { get; set; }
        public IFormFile? File { get; set; }
        public string ImageUrl {  get; set; }
    }
}
