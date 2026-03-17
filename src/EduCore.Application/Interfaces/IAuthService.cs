using EduCore.Application.DTOs;

namespace EduCore.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto> RefreshTokenAsync(string token);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto request);
    Task<UserInfoDto?> GetUserInfoAsync(string userId);
}
