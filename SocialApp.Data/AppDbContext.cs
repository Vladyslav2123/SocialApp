using Microsoft.EntityFrameworkCore;
using SocialApp.Data.Models;

namespace SocialApp.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Post> Posts { get; set; } = null!;
	public DbSet<Like> Likes { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>()
			.HasMany(u => u.Posts)
			.WithOne(p => p.User)
			.HasForeignKey(p => p.UserId);

		modelBuilder.Entity<Like>()
			.HasKey(l => new { l.UserId, l.PostId });

		modelBuilder.Entity<Like>()
			.HasOne(l => l.User)
			.WithMany(u => u.Likes)
			.HasForeignKey(l => l.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Like>()
			.HasOne(l => l.Post)
			.WithMany(p => p.Likes)
			.HasForeignKey(l => l.PostId)
			.OnDelete(DeleteBehavior.Cascade);

		base.OnModelCreating(modelBuilder);
	}
}
