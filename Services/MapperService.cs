using MedicalAPI.Models;
using MedicalAPI.Entities;
namespace MedicalAPI.Services;

public static class MapperService
{
	public static VisitToUser MapVisitToUserResponse(Visit visit)
	{
		return new VisitToUser
		{
			Id = visit.Id,
			Doctor = visit.Doctor != null ? new DoctorToUser
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
	public static DoctorToUser MapDoctorToUserResponse(Doctor doctor)
	{
		return new DoctorToUser
		{
			Id = doctor.Id,
			LastName = doctor.LastName,
			FirstName = doctor.FirstName,
			Patronymic = doctor.Patronymic,
			Specialization = doctor.Specialization,
			Position = doctor.Position,
			ContactNumber = doctor.ContactNumber
		};
	}
}
