using MedicalAPI.Entities;

namespace MedicalAPI.Models;

public class VisitToUser
{
	public Guid Id { get; set; }
	public DoctorToUser? Doctor { get; set; }
	public int Status { get; set; } = 0; // 0 - planned, 1 - completed, 2 - canceled
	public DateTime? Date { get; set; }
	public DateTime? CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}
