using Microsoft.AspNetCore.Mvc;

namespace StoreApi.Models.Account
{
    public class LoginViewModel
    {
        /// <summary>
        /// Email
        /// </summary>
        /// <example>admin@gmail.com</example>
        public string? Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        /// <example>admin1</example>
        public string? Password { get; set; }
    }
}