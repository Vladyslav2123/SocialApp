using Microsoft.AspNetCore.Identity;
using SocialApp.Data.Helpers.Constants;
using SocialApp.Data.Models;

namespace SocialApp.Data.Helpers;

public static class DbInitializer
{

	public static async Task SeedUsersAndRolesAsync ( UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager )
	{
		//Roles
		if (!roleManager.Roles.Any())
		{
			foreach (var roleName in AppRoles.All)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new IdentityRole<int>(roleName));
				}

			}
		}

		//User with Roles
		if (!userManager.Users.Any(u => !string.IsNullOrEmpty(u.Email)))
		{
			var userPassword = "1Password!";
			var newUser = new User()
			{
				UserName = "Edin",
				Email = "edin@gmail.com",
				FullName = "Edin Dzheko",
				ProfilePictureUrl = "https://example.com/edin.jpg",
				EmailConfirmed = true,
			};

			var result = await userManager.CreateAsync(newUser, userPassword);
			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(newUser, AppRoles.User);
			}

			var newUserAdmin = new User()
			{
				UserName = "Admin",
				Email = "admin@gmail.com",
				FullName = "Jack Admin",
				ProfilePictureUrl = "https://example.com/edin.jpg",
				EmailConfirmed = true,
			};

			var resultAdmin = await userManager.CreateAsync(newUserAdmin, userPassword);
			if (resultAdmin.Succeeded)
			{
				await userManager.AddToRoleAsync(newUserAdmin, AppRoles.Admin);
			}

		}
	}

	public static async Task SeedAsync ( AppDbContext context )
	{
		if (!context.Users.Any())
		{
			var users = new List<User>
			{
				new User { FullName = "Alice Smith", ProfilePictureUrl = "https://example.com/alice.jpg" },
				new User { FullName = "Bob Johnson", ProfilePictureUrl = "https://example.com/bob.jpg" }
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
