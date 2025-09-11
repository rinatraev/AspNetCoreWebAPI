using MedicalAPI.Models;
using MedicalAPI.Entities;
namespace MedicalAPI.Services;

public static class MapperService
{
	public static VisitToClientResponseDto MapVisitToClientResponseDto(Visit visit)
	{
		return new VisitToClientResponseDto
		{
			Id = visit.Id,
			Doctor = visit.Doctor != null ? new DoctorToClientDto
			{
				Id = visit.Doctor.Id,
				LastName = visit.Doctor.LastName,
				FirstName = visit.Doctor.FirstName,
				Patronymic = visit.Doctor.Patronymic,
				Specialization = visit.Doctor.Specialization,
				Position = visit.Doctor.Position,
				ContactNumber = visit.Doctor.ContactNumber
			} : null,
			Status = visit.Status,
			Date = visit.Date,
			CreatedAt = visit.CreatedAt,
			UpdatedAt = visit.UpdatedAt
		};
	}
}
