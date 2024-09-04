using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Data.Entities
{
    public class Product

    {

        [Key]
        public int Id { get; set; }
        [Required,StringLength(255)]
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }

    }
}
