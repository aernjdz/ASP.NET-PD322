using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_Hulk.Areas.Admin.Models.Profile;
using Web_Hulk.Data.Entities.Identity;

namespace Web_Hulk.Areas.Admin.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserManager<UserEntity> _userManager;
        public AdminMenuViewComponent(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            ProfileItemViewModel model = new ProfileItemViewModel();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Image = user.Image;

            return View(model);
        }
    }
}
