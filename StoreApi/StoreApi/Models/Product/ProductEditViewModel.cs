namespace StoreApi.Models.Product;
public class ProductEditViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public List<ProductImageViewModel>? PreviousImages { get; set; }
    public List<IFormFile>? NewImages { get; set; }
    public List<int>? ImagesIds { get; set; }
}
