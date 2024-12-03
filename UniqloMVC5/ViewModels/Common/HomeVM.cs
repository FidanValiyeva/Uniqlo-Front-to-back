using System.Collections.Generic;
using UniqloMVC5.ViewModels.Category;
using UniqloMVC5.ViewModels.Product;
using UniqloMVC5.ViewModels.Slider;

namespace UniqloMVC5.ViewModels.Common
{
    public class HomeVM
    {
     public IEnumerable<SliderItemVM> Sliders { get; set; }
     public IEnumerable<ProductItemVM> Products { get; set; }
     public IEnumerable<CategoryItemVM> Categories{ get; set; }


    }
}
