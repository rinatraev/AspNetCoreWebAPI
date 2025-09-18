using MedicalAPI.Entities;

namespace MedicalAPI.Models;

public class CreateVisitFromUser
{
	public required string DoctorId { get; set; }
	public required DateTime Date { get; set; }
	public required DateTime CreatedAt { get; set; }
	
}
