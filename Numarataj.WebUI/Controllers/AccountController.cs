using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Numarataj.Entity.Entities;
using Numarataj.WebUI.ViewModel;

namespace Numarataj.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);
                        ///* TempData["SuccessMessage"] = ""; /*// Başarılı giriş mesajı
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                        TempData["ErrorMessage"] = $"Hesabınız kitlendi, lütfen {timeLeft.Minutes} dakika sonra deneyiniz.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Parolanız hatalı.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Bu email adresiyle bir hesap bulunamadı.";
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
       



    }
}
