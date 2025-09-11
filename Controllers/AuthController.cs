using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MedicalAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
	private readonly AuthService _authService = authService;

	[HttpPost("register")]
	public async Task<ActionResult> RegisterAsync([FromBody] UserDTO userDTO)
	{
		Console.WriteLine("/api/auth/register - Register method started");
		if (userDTO == null || string.IsNullOrEmpty(userDTO.Username) || string.IsNullOrEmpty(userDTO.Password))
		{
			return BadRequest("Invalid user data.");
		}
		Console.WriteLine($"Username:{userDTO.Username} Password:{userDTO.Password}");
		var token = await _authService.RegisterAsync(userDTO);
		if (token == null)
		{
			return BadRequest("User already exists or registration failed.");
		}
		return Ok(token);

	}

	[HttpPost("login")]
	public async Task<ActionResult> LoginAsync([FromBody] UserDTO userDTO)
	{
		Console.WriteLine("/api/auth/login - Login method started");

		if (userDTO == null || string.IsNullOrEmpty(userDTO.Username) || string.IsNullOrEmpty(userDTO.Password))
		{
			return NotFound("User not found");
		}

		var token = await _authService.LoginAsync(userDTO);
		if (token == null)
		{
			return NotFound("User not found");
		}
		return Ok(token);
	}

	[HttpPost("refresh")]
	public async Task<ActionResult> RefreshAccesTokenAsync([FromBody] TokenResponseDTO requestDTO)
	{
		if (requestDTO == null || requestDTO.AccessToken == null && requestDTO.RefreshToken == null)
		{
			return BadRequest("Invalid data sended");
		}

		var responseDTO = await _authService.ValidateTokens(requestDTO);
		if (responseDTO == null) return BadRequest("Refresh command cancelled");
		return Ok(responseDTO);
	}

	[Authorize]
	[HttpGet("secret")]
	public ActionResult Secret()
	{
		return Ok("You get access for secret data");
	}

	[HttpGet("open")]
	public ActionResult Open() 
	{
		return Ok("Open Data");
	}
}
