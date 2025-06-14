using Microsoft.EntityFrameworkCore;
using SocialApp.Data.Models;

namespace SocialApp.Data;

public class AppDbContext : DbContext
{
	public AppDbContext ( DbContextOptions<AppDbContext> options ) : base(options) { }

	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Post> Posts { get; set; } = null!;
	public DbSet<Like> Likes { get; set; } = null!;
	public DbSet<Comment> Comments { get; set; } = null!;
	public DbSet<Favorite> Favorites { get; set; } = null!;
	public DbSet<Report> Reports { get; set; } = null!;
	public DbSet<Story> Stories { get; set; } = null!;

	protected override void OnModelCreating ( ModelBuilder modelBuilder )
	{
		modelBuilder.Entity<User>()
			.HasMany(u => u.Posts)
			.WithOne(p => p.User)
			.HasForeignKey(p => p.UserId);

		modelBuilder.Entity<User>()
			.HasMany(u => u.Stories)
			.WithOne(s => s.User)
			.HasForeignKey(s => s.UserId);

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
			.OnDelete(DeleteBehavior.NoAction);

		// Comment entity configuration can be added here if needed
		modelBuilder.Entity<Comment>()
		   .HasOne(c => c.Post)
		   .WithMany(p => p.Comments)
		   .HasForeignKey(c => c.PostId)
		   .OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<Comment>()
			.HasOne(c => c.User)
		   .WithMany(u => u.Comments)
		   .HasForeignKey(c => c.UserId)
		   .OnDelete(DeleteBehavior.Restrict);

		// Favorite entity configuration can be added here if needed
		modelBuilder.Entity<Favorite>()
		   .HasKey(f => new { f.UserId, f.PostId });

		modelBuilder.Entity<Favorite>()
			.HasOne(f => f.User)
			.WithMany(u => u.Favorites)
			.HasForeignKey(f => f.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Favorite>()
			.HasOne(f => f.Post)
			.WithMany(p => p.Favorites)
			.HasForeignKey(f => f.PostId)
			.OnDelete(DeleteBehavior.NoAction);

		// Report entity configuration can be added here if needed

		modelBuilder.Entity<Report>()
		   .HasKey(r => new { r.UserId, r.PostId });

		modelBuilder.Entity<Report>()
			.HasOne(r => r.User)
			.WithMany(u => u.Reports)
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Report>()
			.HasOne(r => r.Post)
			.WithMany(p => p.Reports)
			.HasForeignKey(r => r.PostId)
			.OnDelete(DeleteBehavior.NoAction);

		base.OnModelCreating(modelBuilder);
	}
}
