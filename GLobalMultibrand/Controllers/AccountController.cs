using GMB.Business.ViewModels.Account;
using GMB.Core.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GLobalMultibrand.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var existUser = await _userManager.FindByNameAsync(model.Username);
            if (existUser is null)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();

            }
            var signResult = await _signInManager.PasswordSignInAsync(existUser, model.Password, model.RememberMe, false);

            if (!signResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Credentials");
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
