using Microsoft.AspNetCore.Identity;

namespace StoreApi.Data.Entities.Identity
{
    public class RoleEntity : IdentityRole<int>
    {
        public  ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}
