namespace MedicalAPI.Models;

public class DoctorToUser
{
	public Guid Id { get; set; }
	public string? LastName { get; set; }
	public string? FirstName { get; set; }
	public string? Patronymic { get; set; }
	public string? Specialization { get; set; }
	public string? Position { get; set; }
	public string? ContactNumber { get; set; }
}
