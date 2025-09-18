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
			new Claim(ClaimTypes.Role, user.Role),
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
		};
		var SecretKey = _configuration["Jwt:Key"];
		var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

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

	public async Task<TokenToUser?> LoginAsync(RequestToLoginByUser requestData)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == requestData.Username);
		if (user == null) return null;

		if ( new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, requestData.Password) == PasswordVerificationResult.Failed) 
			return null;

		if(user.RefreshTokenExpiryTime <= DateTime.UtcNow.AddHours(1))
		{
			user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
			user.RefreshToken = await GenerateRefreshTokenAsync(user);
		}

		TokenToUser token = new TokenToUser
		{
			AccessToken = GenerateAccessToken(user),
			RefreshToken = user.RefreshToken
		};
		return token;
	}

	public async Task<string?> ValidateRefreshToken(TokenRequestByUser requestData)
	{
		User user;
		if (!Guid.TryParse(requestData.UserId, out var userId)) //inversion
		{
			return null;
		}
		else
		{
			user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null) return null;
		}

		//Проверка refresh токена в БД
		if (requestData.RefreshToken != user.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
			return null;
		return GenerateAccessToken(user);
	}
	

}
