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
        if (!_roleManeger.RoleExistsAsync("student").Result)
        {
            IdentityRole role = new IdentityRole();
            role.Name = "student";
            role.NormalizedName = "STUDENT";
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
        if (_userManeger.FindByEmailAsync("rafaellima@gmail.com").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.FirstName = "Rafael";
            user.LastName = "Lima";
            user.UserName = "rafaellima@gmail.com";
            user.Email = "rafaellima@gmail.com";
            user.NormalizedUserName = "RAFAELLIMA@GMAIL.COM";
            user.NormalizedEmail = "RAFAELLIMA@GMAIL.COM";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManeger.CreateAsync(user, "@St4d3nt1").Result;

            if (result.Succeeded)
            {
                _userManeger.AddToRoleAsync(user, "student").Wait();
            }
        }

        if (_userManeger.FindByEmailAsync("admin@gmail.com").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.FirstName = "Administrador";
            user.LastName = "Administrador";
            user.UserName = "admin@gmail.com";
            user.Email = "admin@gmail.com";
            user.NormalizedUserName = "ADMIN@GMAIL.COM";
            user.NormalizedEmail = "ADMIN@GMAIL.COM";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManeger.CreateAsync(user, "@Dm1n4s3r").Result;

            if (result.Succeeded)
            {
                _userManeger.AddToRoleAsync(user, "Admin").Wait();
                _userManeger.AddToRoleAsync(user, "student").Wait();
            }
        }
    }
}
