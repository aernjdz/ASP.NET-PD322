using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Data.Entities.Identity
{
    public class UserEntity
    {
        [Required, StringLength(100)]
        public string UserName { get; set; }
    }
}
