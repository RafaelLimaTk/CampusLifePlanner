using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Application.Mappings;
using CampusLifePlanner.Application.Services;
using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.Infra.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CampusLifePlanner.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection Services,
       IConfiguration configuration)
    {
        Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext)
                    .Assembly.FullName)));

        Services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.OnAppendCookie = cookieContext =>
            options.OnDeleteCookie = cookieContext =>
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/Login";
            options.SlidingExpiration = true;
        });

        Services.AddMvc(option =>
        {
        }).AddCookieTempDataProvider(options =>
        {
            options.Cookie.IsEssential = true;
        });

        Services.AddScoped<IEventRepository, EventRepository>();
        Services.AddScoped<ICourseRepository, CourseRepository>();
        Services.AddScoped<IUserRepository, UserRepository>();

        Services.AddScoped<IEventService, EventService>();
        Services.AddScoped<ICourseService, CourseService>();
        Services.AddScoped<IUserService, UserService>();

        Services.AddScoped<IAuthenticate, AuthenticateService>();
        Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        Services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

        Services.AddAuthorization(options =>
        {
            options.AddPolicy("isAdmin", policy => policy.RequireRole("admin"));
            options.AddPolicy("isStudent", policy => policy.RequireRole("student"));
        });

        return Services;
    }
}
