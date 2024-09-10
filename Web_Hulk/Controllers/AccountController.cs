using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Web_Hulk.Constants;
using Web_Hulk.Data.Entities.Identity;
using Web_Hulk.Models.Account;

namespace Web_Hulk.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IMapper _mapper;
      
        public AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var user = new UserEntity
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email
                };

                var outcome = await _userManager.CreateAsync(user, model.Password);
                if (outcome != null)
                {
                    if (!outcome.Succeeded)
                    {
                        outcome = await _userManager.AddToRoleAsync(user, Roles.User);
                    }
                }
                return RedirectToAction("Index", "Main");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var res = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (res.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Profile));
                }
            }
            ModelState.AddModelError("", "The data is invalid.");

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var id = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(id);

            var model = _mapper.Map<ProfileViewModel>(user);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}