using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Models.Products
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(500, ErrorMessage ="Product name should not exceed 500 characters")]
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = " Choose a category")]
        public int CategoryId { get; set; }
        [Display(Name = "Photo")]
        public List<IFormFile>? Photos { get; set; }
        public SelectList? CategoryList { get; set; }

    }
}
