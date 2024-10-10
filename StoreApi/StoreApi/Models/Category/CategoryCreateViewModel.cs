namespace StoreApi.Models.Category;
    public class CategoryCreateViewModel
    {
        public string Name { get; set; } = String.Empty;
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
    }