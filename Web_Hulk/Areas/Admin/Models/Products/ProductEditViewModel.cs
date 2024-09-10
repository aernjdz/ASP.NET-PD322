using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Areas.Admin.Models.Products
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [RegularExpression(@"^\d+([\,\.]\d{1,})?$", ErrorMessage = "Provide valid price")]
        public string? Price { get; set; }
        public SelectList? CategoryList { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Choose a category")]
        public int CategoryId { get; set; }
        public List<ProductImageViewModel>? Images { get; set; }

        [Display(Name = "New images")]
        public List<IFormFile>? NewImages { get; set; }

        [Display(Name = "Delete image")]
        public List<int>? DeletedPhotoIds { get; set; }
    }
}
