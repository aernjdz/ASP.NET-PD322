using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Web_Hulk.Data.Entities
{
    [Table("tbl_categories")]
    public class CategoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; } = null!;

        [StringLength(255)]
        public string Image { get; set; } = String.Empty;

    }
}
