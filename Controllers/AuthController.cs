using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
	private readonly AuthService _authService = authService;

	[HttpPost("login")]
	public async Task<ActionResult> LoginAsync([FromBody] RequestToLoginByUser requestData)
	{
		TokenToUser? token = await _authService.LoginAsync(requestData);
		if (token == null)	return NotFound("User not found");
		return Ok(token);
	}

	[HttpPost("refresh")]
	public async Task<ActionResult> RefreshAccessTokenAsync([FromBody] TokenRequestByUser requestData)
	{
		var AccesToken = await _authService.ValidateRefreshToken(requestData);
		if (AccesToken == null)	return BadRequest("Validation is failed");
		return Ok(AccesToken);
	}

	[Authorize]
	[HttpGet("secret")]
	public ActionResult ForTestingTokens()
	{
		var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var Role = User.FindFirstValue(ClaimTypes.Role);

		return Ok($"Restricted data: Role: {Role} Id: {UserId}");
	}

	[HttpGet("open")]
	public ActionResult Open() 
	{
		return Ok("Open Data");
	}
}
