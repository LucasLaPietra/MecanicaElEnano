namespace BackendMecanicaElEnano.Models
{
    /// <summary>
    /// Model for JWT token response
    /// </summary>
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
