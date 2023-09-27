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
        if (_userManeger.FindByEmailAsync("andersonlima@gmail.com").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.FirstName = "Anderson";
            user.LastName = "Lima";
            user.UserName = "andersonlima@gmail.com";
            user.Email = "andersonlima@gmail.com";
            user.NormalizedUserName = "ANDERSONLIMA@GMAIL.COM";
            user.NormalizedEmail = "ANDERSONLIMA@GMAIL.COM";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManeger.CreateAsync(user, "@62745263Tk7").Result;

            if (result.Succeeded)
            {
                _userManeger.AddToRoleAsync(user, "User").Wait();
            }
        }

        if (_userManeger.FindByEmailAsync("rafamano123.rl@gmail.com").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.FirstName = "Rafael";
            user.LastName = "Lima";
            user.UserName = "rafamano123.rl@gmail.com";
            user.Email = "rafamano123.rl@gmail.com";
            user.NormalizedUserName = "RAFAMANO123.RL@GMAIL.COM";
            user.NormalizedEmail = "RAFAMANO123.RL@GMAIL.COM";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManeger.CreateAsync(user, "@62745263Tk7").Result;

            if (result.Succeeded)
            {
                _userManeger.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }
}
