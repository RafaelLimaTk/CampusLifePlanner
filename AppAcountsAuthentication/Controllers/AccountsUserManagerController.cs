using AutoMapper;
using CampusLifePlanner.Application.DTOs;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Domain.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppAcountsAuthentication.Controllers
{
    [Authorize(Policy = "isAdmin")]
    public class AccountsUserManagerController : Controller
    {
        private readonly IAuthenticate authentication;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AccountsUserManagerController(IAuthenticate authentication, IUserService userService, IMapper mapper)
        {
            this.authentication = authentication;
            this.userService = userService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View(mapper.Map<List<UserDto>>(await userService.GetAll()));
        }
    }
}
