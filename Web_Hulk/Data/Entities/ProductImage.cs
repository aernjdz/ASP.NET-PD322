namespace Web_Hulk.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Image { get; set; } = string.Empty;
        public int Priority { get; set; }

        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}
