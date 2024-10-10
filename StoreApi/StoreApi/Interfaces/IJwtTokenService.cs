using Microsoft.AspNetCore.Mvc;
using StoreApi.Data.Entities.Identity;

namespace StoreApi.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(UserEntity user);
    }
}
