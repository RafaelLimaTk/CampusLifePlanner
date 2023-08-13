using Azure.Identity;
using CampusLifePlanner.Domain.Account;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace CampusLifePlanner.Infra.Data.Identity;

public class AuthenticateService : IAuthenticate
{
    private readonly UserManager<ApplicationUser> _userManeger;
    private readonly SignInManager<ApplicationUser> _singInManeger;
    public AuthenticateService(SignInManager<ApplicationUser> singInManeger, 
        UserManager<ApplicationUser> userManager)
    {
        _userManeger = userManager;
        _singInManeger = singInManeger;
    }

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

    public async Task<(bool success, string msg)> RegisterUser(string email, string password)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
        };

        var result = await _userManeger.CreateAsync(applicationUser, password);

        if (result.Succeeded)
        {
            await _singInManeger.SignInAsync(applicationUser, isPersistent: false);
        }

        return new(result.Succeeded, result.Errors.Count() == 0  ? null : result.Errors.FirstOrDefault().Description.ToString());
    }
}
