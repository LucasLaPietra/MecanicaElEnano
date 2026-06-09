using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.Services
{
    /// <summary>
    /// Service interface for authentication operations
    /// </summary>
    public interface IAuthService
    {
        Task<Result<TokenResponse>> LoginAsync(LoginDto loginDto);
        Task<Result<TokenResponse>> RegisterAsync(RegisterDto registerDto);
        Task<Result<User>> GetUserByUsernameAsync(string username);
        Task<Result> ChangePasswordAsync(string username, string currentPassword, string newPassword);
    }
}
