using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Models.Products
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public decimal Price { get; set; }

        public SelectList? CategoryList { get; set; }

        [Display(Name="Category")]
        [Required(ErrorMessage = "Choose a category")]
        public int CategoryId { get; set; }
        public List<ProductImageViewModel>? Images { get; set; }

        [Display(Name = "New Images")]
        public List<IFormFile>? NewImages {  get; set; }

        [Display(Name = "Delate Image")]
        public List<int>? DeletedPhotoIds { get; set; }
    }
}
