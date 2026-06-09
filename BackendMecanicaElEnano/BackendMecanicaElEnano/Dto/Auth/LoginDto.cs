using System.ComponentModel.DataAnnotations;

namespace BackendMecanicaElEnano.Dto
{
    /// <summary>
    /// DTO for login request
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }
}
