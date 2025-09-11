using MedicalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MedicalAPI.Entities;
public class Doctor
{
	[Key]
	public Guid Id { get; set; } = Guid.NewGuid();
	public string? LastName { get; set; } = string.Empty; // Family
	public string? FirstName { get; set; } = string.Empty; // Name
	public string? Patronymic { get; set; } = string.Empty; // Otchestvo
	public string Username { get; set; } = string.Empty; // Login
	public string? Password { get; set; } = string.Empty; // Password hash
	public string RefreshToken { get; set; } = string.Empty; // Refresh token
	public DateTime? RefreshTokenExpiryTime { get; set; } // Refresh token expiry time
	public string Role { get; set; } = StaticRoles.Staff; // user, admin
	public string Specialization { get; set; } = string.Empty; // Specialty
	public string Position { get; set; } = string.Empty; // Job title
	public string ContactNumber { get; set; } = string.Empty; // Phone
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public List<Visit> Visits { get; set; } = new List<Visit>();
	
}