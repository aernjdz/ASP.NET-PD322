using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreApi.Data.Entities
{
    public class ProductImageEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Image { get; set; } = string.Empty;
        public int Priority { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public  ProductEntity? Product { get; set; }
    }
}