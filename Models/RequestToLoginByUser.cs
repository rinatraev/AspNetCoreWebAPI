namespace MedicalAPI.Models
{
	public class RequestToLoginByUser
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
	}
}
