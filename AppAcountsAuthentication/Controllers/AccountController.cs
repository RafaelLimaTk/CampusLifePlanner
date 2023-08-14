using CampusLifePlanner.Domain.Account;
using AppAcountsAuthentication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS = CampusLifePlanner.Infra.IoC.Resources;
using Microsoft.AspNetCore.Identity;
using CampusLifePlanner.Infra.Data.Identity;

namespace CampusLifePlanner.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticate _authentication;
    private readonly SignInManager<ApplicationUser> signInManager;

    public AccountController(IAuthenticate authentication, SignInManager<ApplicationUser> signInManager)
    {
        _authentication = authentication;
        this.signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tentativa de Login inválida.");
                return View();
            }
        }
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authentication.RegisterUser(model.Email, model.Password);

            if (result.success)
            {
                return Redirect("/");
            }
            else
            {
                TempData["info"] = result.msg;
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _authentication.Logout();
        return Redirect("/Account/Login");
    }
}
