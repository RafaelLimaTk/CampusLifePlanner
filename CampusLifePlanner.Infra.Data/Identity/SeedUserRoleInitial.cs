using CampusLifePlanner.Domain.Account;
using Microsoft.AspNetCore.Identity;

namespace CampusLifePlanner.Infra.Data.Identity;

public class SeedUserRoleInitial : ISeedUserRoleInitial
{
    private readonly UserManager<ApplicationUser> _userManeger;
    private readonly RoleManager<IdentityRole> _roleManeger;
    public SeedUserRoleInitial(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManeger)
    {
        _userManeger = userManager;
        _roleManeger = roleManeger;
    }

    public void SeedRoles()
    {
        if (!_roleManeger.RoleExistsAsync("User").Result)
        {
            IdentityRole role = new IdentityRole();
            role.Name = "User";
            role.NormalizedName = "USER";
            IdentityResult reloResult = _roleManeger.CreateAsync(role).Result;
        }

        if (!_roleManeger.RoleExistsAsync("Admin").Result)
        {
            IdentityRole role = new IdentityRole();
            role.Name = "Admin";
            role.NormalizedName = "ADMIN";
            IdentityResult reloResult = _roleManeger.CreateAsync(role).Result;
        }
    }

    public void SeedUsers()
    {
        if (_userManeger.FindByEmailAsync("usuario@localhost").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "usuario@localhost";
            user.Email = "usuario@localhost";
            user.NormalizedUserName = "USUARIO@LOCALHOST";
            user.NormalizedEmail = "USUARIO@LOCALHOST";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManeger.CreateAsync(user, "Numsey#2023").Result;

            if (result.Succeeded)
            {
                _userManeger.AddToRoleAsync(user, "User").Wait();
            }
        }

        if (_userManeger.FindByEmailAsync("admin@localhost").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "admin@localhost";
            user.Email = "admin@localhost";
            user.NormalizedUserName = "ADMIN@LOCALHOST";
            user.NormalizedEmail = "ADMIN@LOCALHOST";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManeger.CreateAsync(user, "Numsey#2023").Result;

            if (result.Succeeded)
            {
                _userManeger.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }
}
