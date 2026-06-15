using System.ComponentModel.DataAnnotations;

namespace taskmanager_api.DTOs;

public class RegisterDto
{
    [Required] [MaxLength(30)] public string Username { get; set; } = string.Empty;
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] [MinLength(8)] public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
}