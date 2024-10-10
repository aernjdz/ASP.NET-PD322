using Microsoft.AspNetCore.Mvc;

namespace StoreApi.Models.Product
{
    public class ProductCreateViewModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [BindProperty(Name = "images[]")]
        public List<IFormFile>? Images { get; set; }
    }
}