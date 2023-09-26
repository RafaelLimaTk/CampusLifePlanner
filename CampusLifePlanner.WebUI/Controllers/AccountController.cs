using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.WebUI.Helpers;
using CampusLifePlanner.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RS = CampusLifePlanner.Infra.IoC.Resources;

namespace CampusLifePlanner.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly ICourseService _courseService;
    private readonly IEnrollmentCourseService _enrollmentCourseService;
    private readonly IAuthenticate _authentication;
    private readonly IUtil _util;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(ICourseService courseService, IEnrollmentCourseService enrollmentCourseService,
        IAuthenticate authentication, IUtil util, UserManager<ApplicationUser> userManager)
    {
        _courseService = courseService;
        _enrollmentCourseService = enrollmentCourseService;
        _authentication = authentication;
        _util = util;
        _userManager = userManager;
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
            var result = await _authentication.RegisterUser(model.FirstName, model.LastName, model.Email, model.Password);

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

    [HttpGet]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public JsonResult GetCoursesByUserId(Guid userId)
    {
        var courseIdList = _enrollmentCourseService.GetListByUserId(userId).Select(a => a.CourseId).ToList();
        var courses = _courseService.GetCourseListByCourseId(courseIdList)
            .Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

        return Json(courses);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile imageFile)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.ImgPath))
                {
                    _util.DeleteImage(user.ImgPath, "Perfil");
                }

                var imgPath = await _util.SaveImage(imageFile, "Perfil");

                var userId = _userManager.GetUserId(User);
                await _authentication.UpdateUserProfile(userId, imgPath);
                TempData["success"] = "Imagem de perfil atualizada com sucesso";
                return RedirectToAction("Profile", "Account");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Não foi possivel salvar a imagem";
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUserName(string firstName, string lastName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                TempData["Error"] = "Erro ao tentar atualizar o usuário";
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                TempData["Info"] = "Nome e sobrenome são obrigatórios";
                return RedirectToAction("Profile", "Account");
            }

            user.FirstName = firstName; user.LastName = lastName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Nome do usuário atualizado com sucesso!";
                return RedirectToAction("Profile", "Account");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["Error"] = $"Erro ao tentar atualizar o usuário: {errors}";
                return RedirectToAction("Profile", "Account");
            }
        }
        catch (Exception ex)
        {

            TempData["Error"] = $"Ocorreu um erro inesperado: {ex.Message}";
            return RedirectToAction("Profile", "Account");
        }
    }
}
