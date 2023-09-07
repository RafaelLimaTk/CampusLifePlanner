using CampusLifePlanner.Application.Factories.Implementations;
using CampusLifePlanner.Application.Factories.Interfaces;
using CampusLifePlanner.Application.Interfaces;
using CampusLifePlanner.Application.Interfaces.Base;
using CampusLifePlanner.Application.Mappings;
using CampusLifePlanner.Application.Services;
using CampusLifePlanner.Domain.Account;
using CampusLifePlanner.Domain.Interfaces;
using CampusLifePlanner.Infra.Data.Context;
using CampusLifePlanner.Infra.Data.Identity;
using CampusLifePlanner.Infra.Data.Repositories;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Claims;

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
        Services.AddScoped<IEnrollmentCourseRepository, EnrollmentCourseRepository>();
        Services.AddScoped<IEventLogRepository, EventLogRepository>();

        Services.AddScoped<IEventService, EventService>();
        Services.AddScoped<ICourseService, CourseService>();
        Services.AddScoped<IEnrollmentCourseService, EnrollmentCourseService>();
        Services.AddScoped<IEventLogService, EventLogService>();

        Services.AddScoped<IEventDtoFactory, EventDtoFactory>();

        Services.AddScoped<IAuthenticate, AuthenticateService>();
        Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        Services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

        Services.AddAuthorization(options =>
        {
            options.AddPolicy("HangfireDashboard",
                builder => builder.RequireClaim(ClaimTypes.Role, "Admin"));
            options.AddPolicy("isAdmin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("isStudent", policy => policy.RequireRole("student"));
        });

        Services.AddFluentMigratorCore()
            .ConfigureRunner(x => x.AddSqlServer()
            .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            .AddLogging(y => y.AddFluentMigratorConsole());

        return Services;
    }
}
