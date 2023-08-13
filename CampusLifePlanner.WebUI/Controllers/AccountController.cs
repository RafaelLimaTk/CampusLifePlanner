using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RS = CampusLifePlanner.Infra.IoC.Resources;

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
                TempData["warning"] = RS.Common.EX_MSG_INVALID_USER.Replace("{0}", RS.Common.GENERAL_PAGE_LBL_USER).Replace("{1}", RS.Common.GENERAL_PAGE_LBL_PASSWORD);
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
