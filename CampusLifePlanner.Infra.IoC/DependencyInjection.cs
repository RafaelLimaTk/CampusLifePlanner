using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Application.Mappings;
using CampusLifePlanner.Application.Services;
using CampusLifePlanner.Application.Services.Base;
using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.Domain.Entities;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Domain.Interfaces.Base;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.Infra.Data.Repositories;
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

        Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        Services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Account/Login";
            options.Cookie.Name = "CampusLifePlanner";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.SlidingExpiration = true;
        });

        Services.AddScoped<IEventRepository, EventRepository>();
        Services.AddScoped<ICourseRepository, CourseRepository>();

        Services.AddScoped<IEventService, EventService>();
        Services.AddScoped<ICourseService, CourseService>();

        Services.AddScoped<IAuthenticate, AuthenticateService>();
        Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        Services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

        return Services;
    }
}
