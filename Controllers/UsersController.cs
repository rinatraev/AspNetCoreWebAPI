
using MedicalAPI.Data;
using MedicalAPI.Entities;
using MedicalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

	[HttpGet("{userId:guid}/visits")]
	[Authorize(Roles = StaticRoles.User)]
	public async Task<ActionResult> GetAllVisits([FromRoute] Guid? userId)
	{
		if (userId == null)
			return BadRequest("UserId is required.");

		var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

		if (user == null) return NotFound($"User with ID {userId} not found.");

		//var visits = await _context.Visits.Include(v => v.Doctor).Where(u => u.UserId == userId).ToListAsync();
		var visits = user.Visits;
		if (visits == null) return NotFound();

		return Ok(visits);
	}
}