using System.ComponentModel.DataAnnotations;
namespace MedicalAPI.Entities;
public class Visit
{
	[Key]
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public User User { get; set; }
	public Guid DoctorId { get; set; }
	public Doctor Doctor { get; set; }
	public int Status { get; set; } = 0; // 0 - planned, 1 - completed, 2 - canceled
	public DateTime? Date { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; set; } = null;
}
