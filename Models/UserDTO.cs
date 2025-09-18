using MedicalAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MedicalAPI.Models;

public class UserDto
{
		public Guid Id { get; set; }
		public string? LastName { get; set; } = null; // Family
		public string? FirstName { get; set; } = null; // Name
		public string? Patronymic { get; set; } = null; // Otchestvo
		public string? Username { get; set; } = null; // Login
		public string? Password { get; set; } = null; // Password hash
		public string? ContactPhone { get; set; } = null;
		public int Gender { get; set; } = 0; // 0 -	undefined, 1 - m, 2 - f
		public DateOnly? BirthDate { get; set; } = null;
		public DateTime? CreatedAt { get; set; } = null;
}
