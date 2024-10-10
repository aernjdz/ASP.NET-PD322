using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StoreApi.Data.Entities.Identity;
using StoreApi.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreApi.Services
{
    public class JwtTokenService(IConfiguration _configuration,
         UserManager<UserEntity> userManager) : IJwtTokenService
    {
        public async Task<string> GenerateToken(UserEntity user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
            var securityKey = new SymmetricSecurityKey(key);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, $"{user.LastName} {user.FirstName}"),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new Claim(ClaimTypes.Role, user.UserRoles.FirstOrDefault()?.Role.Name ?? "User")
        };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim("roles", role));

            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);



            var jwt = new JwtSecurityToken(
                signingCredentials: creds,
                claims: claims,
                expires: DateTime.Now.AddDays(20)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}