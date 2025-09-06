using Microsoft.EntityFrameworkCore;
using MedicalAPI.Entities;
namespace MedicalAPI.Data;

public class MedicalDbContext: DbContext
{
	public MedicalDbContext(DbContextOptions<MedicalDbContext> options) : base(options) { }
	public DbSet<User> Users { get; set; } = null!;
}
