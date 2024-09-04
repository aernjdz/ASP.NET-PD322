using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Models.Categories
{
    public class CategoryEditViewModel
    {
       public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Image {  get; set; }
        [Display(Name = "Choose an image  from the PC")]
        public IFormFile? NewImage { get; set; }
    }
}
