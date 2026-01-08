namespace BackendWebApi.Services;

using BackendWebApi.DTOs;
using BackendWebApi.Models.Identity;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<Usuario> GetUserByIdAsync(int userId);
}
