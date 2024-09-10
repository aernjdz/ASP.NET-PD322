using Microsoft.AspNetCore.Identity;

namespace Web_Hulk.Data.Entities.Identity
{
    public class RoleEntity : IdentityRole<int>
    {
        public ICollection<UserRoleEntity> Roles { get; set; } = [];
    }
}
