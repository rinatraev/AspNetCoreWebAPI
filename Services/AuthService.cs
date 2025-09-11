using MedicalAPI.Data;
using MedicalAPI.Entities;
using MedicalAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalAPI.Services;

public class AuthService
{
	private readonly MedicalDbContext _context;
	private readonly IConfiguration _configuration;
	public AuthService(MedicalDbContext context, IConfiguration configuration)
	{
		_context = context;
		_configuration = configuration;
	}

	private string GenerateAccessToken(User user) 
	{
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(ClaimTypes.Role, user.Role)
		};
		var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
			claims: claims,
			expires: DateTime.UtcNow.AddHours(1),
			signingCredentials: creds);

		var response = new JwtSecurityTokenHandler().WriteToken(token);
		return response;
	}
	private async Task<string> GenerateRefreshTokenAsync(User user)
	{
		var randomnumber = new byte[32];
		var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
		rng.GetBytes(randomnumber);
		var response =  Convert.ToBase64String(randomnumber);
		user.RefreshToken = response;
		user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
		await _context.SaveChangesAsync();
		return response;
	}
	private bool ValidateAccessToken(string accessToken)
	{
		var handler = new JwtSecurityTokenHandler();
		var signingKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

		SecurityToken validatedToken;
		ClaimsPrincipal claims;
		var validationParams = new TokenValidationParameters()
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(signingKey),
			ValidateLifetime = true // проверка, что не истёк
		};

		try
		{
			claims = handler.ValidateToken(accessToken, validationParams, out validatedToken);

			//var role = claims.FindFirst("Role")?.Value;
			//var rawUserId = claims.FindFirst("UserID")?.Value;

			// Проверяем роль
			//if (string.IsNullOrWhiteSpace(role) || !allowedRoles.Contains(role))
			//throw new SecurityTokenException($"Недопустимая роль в токене: {role}");

			// Проверяем UserID: должно парситься в Guid и существовать в БД
			//if (!Guid.TryParse(rawUserId, out var userId) || !userRepo.Exists(userId))
			//throw new SecurityTokenException($"Невалидный или несуществующий UserID: {rawUserId}");
		}
		catch (SecurityTokenException)
		{

			return false;  // false, because token is not valid
		}
		return true;
		
		

	}

	public async Task<TokenResponseDTO?>RegisterAsync(UserDTO userDTO)
	{
		Console.WriteLine("RegisterAsync ServiceMethod started");

		if ( await _context.Users.AnyAsync(u => u.Username == userDTO.Username))
		{
			return null;
		}

		User user = new User();

		var HashedPassword = new PasswordHasher<User>().HashPassword(user, userDTO.Password);
		user.Password = HashedPassword;

		user.Username = userDTO.Username;
		user.Password = HashedPassword;

		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		return new TokenResponseDTO
		{
			AccessToken = GenerateAccessToken(user),
			RefreshToken = await GenerateRefreshTokenAsync(user)
		};
	}

	public async Task<TokenResponseDTO?> LoginAsync(UserDTO userDTO)
	{
		Console.WriteLine("LoginAsync ServiceMethod started");

		var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);
		if (user == null || user.Password == null) return null;

		if ( new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, userDTO.Password) == PasswordVerificationResult.Failed) return null;

		// Generate tokens
		return new TokenResponseDTO
		{
			AccessToken = GenerateAccessToken(user),
			RefreshToken = await GenerateRefreshTokenAsync(user)
		};
	}

	public async Task<TokenResponseDTO?> ValidateTokens(TokenResponseDTO tokens)
	{
		if(ValidateAccessToken(tokens.AccessToken)) return null;

		//Проверка refresh токена в БД
		var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == tokens.RefreshToken);
		if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
		{
			return null;
		}

		tokens.AccessToken = GenerateAccessToken(user);
		return tokens;
	}
	

}
