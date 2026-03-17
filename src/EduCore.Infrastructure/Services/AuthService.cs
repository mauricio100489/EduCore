using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EduCore.Application.DTOs;
using EduCore.Application.Interfaces;
using EduCore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EduCore.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.IsActive)
            return new LoginResponseDto { Success = false, ErrorMessage = "Credenciales inv\u00e1lidas" };

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            return new LoginResponseDto { Success = false, ErrorMessage = "Credenciales inv\u00e1lidas" };

        var token = await GenerateJwtTokenAsync(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        return new LoginResponseDto
        {
            Success = true,
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddHours(
                double.Parse(_configuration["JwtSettings:ExpirationInHours"] ?? "8")),
            User = new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Role = roles.FirstOrDefault() ?? "User",
                Permissions = claims.Where(c => c.Type == "Permission")
                    .Select(c => c.Value).ToList(),
                Claims = claims.GroupBy(c => c.Type).ToDictionary(g => g.Key, g => string.Join(",", g.Select(c => c.Value)))
            }
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return new LoginResponseDto { Success = false, ErrorMessage = "El correo ya est\u00e1 registrado" };

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return new LoginResponseDto
            {
                Success = false,
                ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description))
            };

        if (!await _roleManager.RoleExistsAsync(request.Role))
            await _roleManager.CreateAsync(new IdentityRole(request.Role));

        await _userManager.AddToRoleAsync(user, request.Role);

        var defaultPermissions = GetDefaultPermissionsForRole(request.Role);
        foreach (var permission in defaultPermissions)
            await _userManager.AddClaimAsync(user, new Claim("Permission", permission));

        return await LoginAsync(new LoginRequestDto { Email = request.Email, Password = request.Password });
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string token)
    {
        var principal = GetPrincipalFromExpiredToken(token);
        var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return new LoginResponseDto { Success = false, ErrorMessage = "Token inv\u00e1lido" };

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return new LoginResponseDto { Success = false, ErrorMessage = "Token expirado" };

        var newToken = await GenerateJwtTokenAsync(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto
        {
            Success = true,
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expiration = DateTime.UtcNow.AddHours(
                double.Parse(_configuration["JwtSettings:ExpirationInHours"] ?? "8"))
        };
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return false;

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded;
    }

    public async Task<UserInfoDto?> GetUserInfoAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        return new UserInfoDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Role = roles.FirstOrDefault() ?? "User",
            Permissions = claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList(),
            Claims = claims.GroupBy(c => c.Type).ToDictionary(g => g.Key, g => string.Join(",", g.Select(c => c.Value)))
        };
    }

    private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        foreach (var claim in userClaims)
            claims.Add(new Claim(claim.Type, claim.Value));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]
                ?? "EduCore_SuperSecretKey_2024_MinLength32Characters!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"] ?? "EduCore.API",
            audience: _configuration["JwtSettings:Audience"] ?? "EduCore.Web",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                double.Parse(_configuration["JwtSettings:ExpirationInHours"] ?? "8")),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]
                    ?? "EduCore_SuperSecretKey_2024_MinLength32Characters!")),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            return null;

        return principal;
    }

    private static List<string> GetDefaultPermissionsForRole(string role) => role switch
    {
        "Admin" => new List<string>
        {
            "students.view", "students.create", "students.edit", "students.delete",
            "teachers.view", "teachers.create", "teachers.edit", "teachers.delete",
            "courses.view", "courses.create", "courses.edit", "courses.delete",
            "enrollments.view", "enrollments.create", "enrollments.edit", "enrollments.delete",
            "grades.view", "grades.create", "grades.edit", "grades.delete",
            "invoices.view", "invoices.create", "invoices.edit", "invoices.delete",
            "payments.view", "payments.create",
            "reports.view", "settings.manage", "users.manage"
        },
        "Director" => new List<string>
        {
            "students.view", "students.create", "students.edit",
            "teachers.view", "teachers.create", "teachers.edit",
            "courses.view", "courses.create", "courses.edit",
            "enrollments.view", "enrollments.create", "enrollments.edit",
            "grades.view", "grades.create", "grades.edit",
            "invoices.view", "invoices.create",
            "payments.view", "payments.create", "reports.view"
        },
        "Teacher" => new List<string>
        {
            "students.view", "courses.view", "grades.view", "grades.create", "grades.edit"
        },
        "Secretary" => new List<string>
        {
            "students.view", "students.create", "students.edit",
            "enrollments.view", "enrollments.create", "enrollments.edit",
            "courses.view", "invoices.view"
        },
        "Accountant" => new List<string>
        {
            "students.view", "invoices.view", "invoices.create", "invoices.edit",
            "payments.view", "payments.create", "reports.view"
        },
        _ => new List<string> { "students.view", "courses.view", "grades.view" }
    };
}
