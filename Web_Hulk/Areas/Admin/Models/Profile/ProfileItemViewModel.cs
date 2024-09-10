using Microsoft.AspNetCore.Mvc;

namespace Web_Hulk.Areas.Admin.Models.Profile
{
    public class ProfileItemViewModel
    {
        public string? Image { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; }
    }
}
