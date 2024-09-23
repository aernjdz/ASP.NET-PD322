
using Web_Hulk.Models.Categories;
namespace Web_Hulk.Models.Products
{
        public class ProductHomeViewModel
        {
            public IList<ProductItemViewModel> Products { get; set; }
            public IList<CategoryItemViewModel> Categories { get; set; }
            public ProductSearchViewModel? Search { get; set; }
            public PaginationViewModel? Pagination { get; set; }
            public int Count { get; set; }
        }
    }