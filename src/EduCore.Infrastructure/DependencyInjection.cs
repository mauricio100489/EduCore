using System.Text;
using EduCore.Application.Interfaces;
using EduCore.Application.Services;
using EduCore.Domain.Interfaces;
using EduCore.Infrastructure.Data;
using EduCore.Infrastructure.Identity;
using EduCore.Infrastructure.Repositories;
using EduCore.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EduCore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EduCoreDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(EduCoreDbContext).Assembly.FullName)));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<EduCoreDbContext>()
        .AddDefaultTokenProviders();

        var jwtKey = configuration["JwtSettings:SecretKey"] ?? "EduCore_SuperSecretKey_2024_MinLength32Characters!";
        var key = Encoding.UTF8.GetBytes(jwtKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"] ?? "EduCore.API",
                ValidAudience = configuration["JwtSettings:Audience"] ?? "EduCore.Web",
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IGradeLevelService, GradeLevelService>();
        services.AddScoped<IAcademicPeriodService, AcademicPeriodService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<IGradeService, GradeService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IInstituteService, InstituteService>();
        services.AddScoped<IInstitutePeriodService, InstitutePeriodService>();
        services.AddScoped<IInstituteGradeService, InstituteGradeService>();
        services.AddScoped<IInstituteShiftService, InstituteShiftService>();
        services.AddScoped<IInstituteSectionService, InstituteSectionService>();

        return services;
    }

    public static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EduCoreDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.EnsureCreatedAsync();

        string[] roles = { "Admin", "Director", "Teacher", "Secretary", "Accountant", "Parent", "Student" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (await userManager.FindByEmailAsync("admin@educore.com") is null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@educore.com",
                Email = "admin@educore.com",
                FirstName = "Administrador",
                LastName = "Sistema",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");

            var permissions = new[]
            {
                "students.view", "students.create", "students.edit", "students.delete",
                "teachers.view", "teachers.create", "teachers.edit", "teachers.delete",
                "courses.view", "courses.create", "courses.edit", "courses.delete",
                "enrollments.view", "enrollments.create", "enrollments.edit", "enrollments.delete",
                "grades.view", "grades.create", "grades.edit", "grades.delete",
                "invoices.view", "invoices.create", "invoices.edit", "invoices.delete",
                "payments.view", "payments.create",
                "reports.view", "settings.manage", "users.manage"
            };
            foreach (var perm in permissions)
                await userManager.AddClaimAsync(admin, new System.Security.Claims.Claim("Permission", perm));
        }
    }
}
