
using MedicalAPI.Data;
using MedicalAPI.Entities;
using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedicalAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	private readonly MedicalDbContext _context;
	public UsersController(MedicalDbContext context)
	{
		_context = context;
	}

	[HttpPost("register")]
	public async Task<ActionResult> RegisterUserAsync([FromBody] RequestToLoginByUser userData)
	{

		bool isUsernameExists = await _context.Users.AnyAsync(u => u.Username == userData.Username);
		if (isUsernameExists) return BadRequest("User already exists or registration failed.");

		User user = new User();

		user.Password = new PasswordHasher<User>().HashPassword(user, userData.Password);
		user.Username = userData.Username;

		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();
		return Ok();
	}

	[HttpGet("visits")]
	[Authorize(Roles = StaticRoles.User)]
	public async Task<ActionResult> GetVisitsByUserId()
	{
		if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId)) //inversion
		{
			return BadRequest("Invalid userId");
		}
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

		if (user == null) return BadRequest($"User with ID {userId.ToString()} not found");

		var visits = await _context.Visits.Include(v => v.Doctor).Where(v => v.UserId == userId && !v.IsDeletedByUser).ToListAsync();
		var mappedVisits = visits.Select(v => MapperService.MapVisitToUserResponse(v)).ToList();
		return Ok(mappedVisits);
	}

	[HttpDelete("visits")]
	[Authorize(Roles = StaticRoles.User)]
	public async Task<ActionResult> DeleteVisitById([FromBody] string visitId)
	{
		if (!Guid.TryParse(visitId, out Guid parsedVisitID)) //inversion
		{
			return BadRequest("Invalid visit Id");
		}

		var visit = await _context.Visits.FirstOrDefaultAsync(v => v.Id == parsedVisitID);

		if (visit == null)
			return NotFound($"Visit with ID {visitId} not found.");

		if (visit.UserId.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier))
			return Forbid("You are not authorized to delete this visit.");

		if (visit.Status == 1) visit.Status = 2; // 0 - planned, 1 - completed, 2 - canceled
		visit.IsDeletedByUser = true;
		visit.UpdatedAt = DateTime.UtcNow;

		await _context.SaveChangesAsync();
		return Ok("Visit was deleted");
	}

	[HttpPost("visits")]
	[Authorize(Roles = StaticRoles.User)]
	public async Task<ActionResult> CreateVisit([FromBody] CreateVisitFromUser visit)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		_context.Visits.Add(
			new Visit()
			{
				DoctorId = Guid.Parse(visit.DoctorId),
				Date = visit.Date,
				CreatedAt = visit.CreatedAt,
				UserId = Guid.Parse(userId),
			}
			);
		await _context.SaveChangesAsync();
		return Ok();
	}

	[HttpGet("doctors")]
	[Authorize(Roles = StaticRoles.User)]
	public async Task<ActionResult> GetDoctors()
	{
		var doctors = await _context.Doctors.Select(d => MapperService.MapDoctorToUserResponse(d)).ToListAsync();
		return Ok(doctors);
	}
}