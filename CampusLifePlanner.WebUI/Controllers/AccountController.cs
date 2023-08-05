using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusLifePlanner.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticate _authentication;
    public AccountController(IAuthenticate authentication)
    {
        _authentication = authentication;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authentication.Authentication(model.Email, model.Password);

            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.(password must be strong).");
                
            }
        }
        return View(model);
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

            if (result)
            {
                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Houve um problema durante o registro. Verifique se a senha é forte o suficiente.");
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
