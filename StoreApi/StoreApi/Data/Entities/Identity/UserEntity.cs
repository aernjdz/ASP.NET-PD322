using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StoreApi.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<int>
    {
        [StringLength(255)]
        public string? Image { get; set; }
        [StringLength(100)]
        public string? LastName { get; set; }
        [StringLength(100)]
        public string? FirstName { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; } = [];

    }
}