using MedicalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MedicalAPI.Entities;

public class User
{
	[Key]
	public Guid Id { get; set; } = Guid.NewGuid();
	public string? LastName { get; set; } = string.Empty; // Family
	public string? FirstName { get; set; } = string.Empty; // Namee
	public string? Patronymic { get; set; } = string.Empty; // Otchestvo
	public string Username { get; set; } = string.Empty; // Login
	public string? Password { get; set; } = string.Empty; // Password hash
	public string RefreshToken { get; set; } = string.Empty; // Refresh token
	public DateTime? RefreshTokenExpiryTime { get; set; } // Refresh token expiry time
	public string Role { get; set; } = StaticRoles.User; // user, admin
	public string? ContactPhone { get; set; } // Phone number
	public int Gender { get; set; } = 0; // 0 -	undefined, 1 - m, 2 - f
	public DateOnly? BirthDate { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;	
	public List<Visit> Visits { get; set; } = new List<Visit>();
}
