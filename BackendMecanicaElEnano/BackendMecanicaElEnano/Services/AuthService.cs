using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BackendMecanicaElEnano.Common;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BackendMecanicaElEnano.Services
{
    /// <summary>
    /// Service for handling authentication operations
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly MecanicaContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            MecanicaContext context,
            IConfiguration configuration,
            ILogger<AuthService> logger,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<TokenResponse>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", loginDto.Username);

                if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    return Result.Failure<TokenResponse>("Usuario y contraseña son requeridos");
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

                if (user == null)
                {
                    _logger.LogWarning("Login failed - user not found: {Username}", loginDto.Username);
                    return Result.Failure<TokenResponse>("Usuario o contraseña incorrectos");
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("Login failed - user is inactive: {Username}", loginDto.Username);
                    return Result.Failure<TokenResponse>("Usuario desactivado");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    loginDto.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    _logger.LogWarning("Login failed - invalid password for user: {Username}", loginDto.Username);
                    return Result.Failure<TokenResponse>("Usuario o contraseña incorrectos");
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                _logger.LogInformation("User {Username} logged in successfully", loginDto.Username);

                return Result.Success(new TokenResponse
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(24),
                    Username = user.Username
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", loginDto.Username);
                return Result.Failure<TokenResponse>("Error al iniciar sesión");
            }
        }

        public async Task<Result<TokenResponse>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Registration attempt for user: {Username}", registerDto.Username);

                // Validate input
                if (string.IsNullOrWhiteSpace(registerDto.Username))
                {
                    return Result.Failure<TokenResponse>("El nombre de usuario es requerido");
                }

                if (registerDto.Username.Length < 3 || registerDto.Username.Length > 50)
                {
                    return Result.Failure<TokenResponse>("El nombre de usuario debe tener entre 3 y 50 caracteres");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Password) || registerDto.Password.Length < 6)
                {
                    return Result.Failure<TokenResponse>("La contraseña debe tener al menos 6 caracteres");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Email))
                {
                    return Result.Failure<TokenResponse>("El email es requerido");
                }

                // Check if username exists
                var usernameExists = await _context.Users
                    .AnyAsync(u => u.Username == registerDto.Username);

                if (usernameExists)
                {
                    _logger.LogWarning("Registration failed - username already exists: {Username}", registerDto.Username);
                    return Result.Failure<TokenResponse>("El nombre de usuario ya está en uso");
                }

                // Check if email exists
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == registerDto.Email);

                if (emailExists)
                {
                    _logger.LogWarning("Registration failed - email already exists: {Email}", registerDto.Email);
                    return Result.Failure<TokenResponse>("El email ya está en uso");
                }

                // Create new user
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    FullName = registerDto.FullName,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Role = "User"
                };

                // Hash password
                user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                _logger.LogInformation("User {Username} registered successfully", registerDto.Username);

                return Result.Success(new TokenResponse
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(24),
                    Username = user.Username
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user: {Username}", registerDto.Username);
                return Result.Failure<TokenResponse>("Error al registrar usuario");
            }
        }

        public async Task<Result<User>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    return Result.Failure<User>("Usuario no encontrado");
                }

                return Result.Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user: {Username}", username);
                return Result.Failure<User>("Error al obtener usuario");
            }
        }

        public async Task<Result> ChangePasswordAsync(string username, string currentPassword, string newPassword)
        {
            try
            {
                _logger.LogInformation("Password change attempt for user: {Username}", username);

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    return Result.Failure("La nueva contraseña debe tener al menos 6 caracteres");
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null)
                {
                    return Result.Failure("Usuario no encontrado");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    currentPassword);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    _logger.LogWarning("Password change failed - invalid current password for user: {Username}", username);
                    return Result.Failure("La contraseña actual es incorrecta");
                }

                user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Password changed successfully for user: {Username}", username);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user: {Username}", username);
                return Result.Failure("Error al cambiar contraseña");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
