namespace MedicalAPI.Models;

public class TokenRequestByUser
{
	public required string RefreshToken { get; set; }
	public required string UserId { get; set; }
}
