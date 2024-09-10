using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Web_Hulk.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<int>
    {
        [Required, StringLength(100)]
        public string? FirstName { get; set; }

        [Required, StringLength(100)]
        public string? LastName { get; set; }
        [StringLength(100)]

        public string? Image { get; set; }

        public ICollection<UserRoleEntity> uSerRoles { get; set; } = [];
    }
}
