using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.Infra.Data.Migrations;
using Microsoft.AspNetCore.Identity;

namespace CampusLifePlanner.Infra.Data.Identity;

public class AuthenticateService : IAuthenticate
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _singInManeger;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AuthenticateService(SignInManager<ApplicationUser> singInManeger,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _singInManeger = singInManeger;
    }

    public RoleManager<ApplicationUser> RoleManager { get; }

    public async Task<bool> Authentication(string email, string password)
    {
        var result = await _singInManeger.PasswordSignInAsync(email,
            password, false, lockoutOnFailure: false);

        return result.Succeeded;
    }

    public async Task Logout()
    {
        await _singInManeger.SignOutAsync();
    }

    public async Task<(bool success, string msg)> RegisterUser(string firstName, string lastName, string email, string password)
    {

        var applicationUser = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(applicationUser, password);

        if (applicationUser != null)
        {
            if (!await _roleManager.RoleExistsAsync("student"))
                await _roleManager.CreateAsync(new IdentityRole("student"));

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            await _userManager.AddToRoleAsync(applicationUser, "student");
            await _userManager.AddToRoleAsync(applicationUser, "Admin");
        }
        else
            throw new Exception("Ocorreu um erro ao tentar inserir o nível de permissão no usuário");

        if (result.Succeeded)
        {
            await _singInManeger.SignInAsync(applicationUser, isPersistent: false);
        }

        return new(result.Succeeded, result.Errors.Count() == 0 ? null : result.Errors.FirstOrDefault().Description.ToString());
    }

    public async Task<bool> UpdateUserProfile(string userId, string imgPath)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.ImgPath = imgPath;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        return false;
    }

}
