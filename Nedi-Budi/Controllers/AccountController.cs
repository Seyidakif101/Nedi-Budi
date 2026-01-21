using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nedi_Budi.Enums;
using Nedi_Budi.Models;
using Nedi_Budi.ViewModels.AccountViewModels;
using System.Threading.Tasks;

namespace Nedi_Budi.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AppUser user = new()
            {
                UserName=vm.UserName,
                Email=vm.Email
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user, RoleEnum.Member.ToString());
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user =await _userManager.FindByEmailAsync(vm.Email);
            if(user is null)
            {
                ModelState.AddModelError("", "Error!!!");
                return View(vm);

            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error!!!");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));

        }
    }
}
