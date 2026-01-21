using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nedi_Budi.Enums;
using Nedi_Budi.Models;
using Nedi_Budi.ViewModels.AccountViewModels;

namespace Nedi_Budi.Helper
{
    public class DbContextRole
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AdminVM _admin;

        public DbContextRole(UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _admin = _configuration.GetSection("AdminSettings").Get<AdminVM>() ?? new();
        }
        public async Task RoleDatabase()
        {
            await CreateRole();
            await CreateAdmin();
        }
        private async Task CreateAdmin()
        {
            AppUser user = new()
            {
                UserName = _admin.UserName,
                Email = _admin.Email
            };
            var result = await _userManager.CreateAsync(user, _admin.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleEnum.Admin.ToString());
            }
        }
        private async Task CreateRole()
        {
            foreach(var role in Enum.GetNames(typeof(RoleEnum)))
            {
                await _roleManager.CreateAsync(new()
                {
                    Name = role
                });
            }
        }
    }
}
