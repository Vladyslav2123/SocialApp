using Microsoft.AspNetCore.Mvc;
using SocialApp.Data;
using SocialApp.Data.Models;
using SocialApp.ViewModels.Stories;

namespace SocialApp.Controllers;

public class StoriesController : Controller
{
	private readonly AppDbContext _dbContext;

	public StoriesController ( AppDbContext dbContext )
	{
		_dbContext = dbContext;
	}

	[HttpPost]
	public async Task<IActionResult> CreateStory ( StoryVM storyVM )
	{
		int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

		var newStory = new Story
		{
			UserId = userId,
			IsDeleted = false, // Default value for new stories
			CreatedAt = DateTime.UtcNow,
			ImageUrl = null // Initialize with null; will be set if an image is uploaded
		};

		// Handle image upload if provided
		if (storyVM.Image != null && storyVM.Image.Length > 0)
		{
			string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			if (storyVM.Image.ContentType.Contains("image"))
			{
				string rootPathImage = Path.Combine(rootPath, "images/stories");
				Directory.CreateDirectory(rootPathImage); // Ensure the directory exists

				string fileName = $"{Guid.NewGuid()}_{storyVM.Image.FileName}";
				string filePath = Path.Combine(rootPathImage, fileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
					await storyVM.Image.CopyToAsync(fileStream);

				newStory.ImageUrl = $"/images/stories/{fileName}"; // Set the image URL for the post
			}
		}

		await _dbContext.Stories.AddAsync(newStory);
		await _dbContext.SaveChangesAsync();
		return RedirectToAction("Index", "Home"); // Redirect to the home page or wherever appropriate
	}
}
