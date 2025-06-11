using SocialApp.Data.Models;

namespace SocialApp.Data.Helpers;

public static class DbInitializer
{
	public static async Task SeedAsync(AppDbContext context)
	{
		if (!context.Users.Any())
		{
			var users = new List<User>
			{
				new User { Fullname = "Alice Smith", ProfilePictureUrl = "https://example.com/alice.jpg" },
				new User { Fullname = "Bob Johnson", ProfilePictureUrl = "https://example.com/bob.jpg" }
			};
			await context.Users.AddRangeAsync(users);
			await context.SaveChangesAsync();
		}
		if (!context.Posts.Any())
		{
			var posts = new List<Post>
			{
				new Post {
					Content = "Hello World!",
					NrOfReports = 0,
					ImageUrl = "https://example.com/image1.jpg",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow,
					UserId = 1 },
				new Post {
					Content = "My first post!",
					NrOfReports = 0,
					ImageUrl = "https://example.com/image2.jpg",
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow,
					UserId = 2 }
			};
			await context.Posts.AddRangeAsync(posts);
			await context.SaveChangesAsync();
		}
	}
}
