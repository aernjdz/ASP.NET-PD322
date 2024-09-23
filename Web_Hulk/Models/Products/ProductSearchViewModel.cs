using System.ComponentModel.DataAnnotations;
using Web_Hulk.Models.Categories;
namespace Web_Hulk.Models.Products
{
    public class ProductSearchViewModel
    {
        public string? Name { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public int? Page { get; set; }
        public int PageSize { get; set; } = 12;
        public List<CategoryItemViewModel> Categories { get; set; }
        public List<ProductItemViewModel> Products { get; set; }
        public string? SortBy { get; set; }
    }
}