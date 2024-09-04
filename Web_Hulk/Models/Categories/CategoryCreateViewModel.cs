using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Models.Categories
{
    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        //[Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Select a photo on your PC")]
        [Required(ErrorMessage = "Photo is required")]
        [DataType(DataType.Upload)]
        public required IFormFile Image { get; set; }
    }
}
