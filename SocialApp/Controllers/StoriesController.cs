using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Helpers.Enums;
using SocialApp.Data.Models;
using SocialApp.Data.Services;
using SocialApp.ViewModels.Stories;

namespace SocialApp.Controllers;

public class StoriesController : Controller
{
	private readonly IStoriesService _storiesService;
	private readonly IFilesService _fileService; // Assuming you have a file service for handling image uploads

	public StoriesController ( IStoriesService storiesService, IFilesService filesService )
	{
		_storiesService = storiesService ?? throw new ArgumentNullException(nameof(storiesService));
		_fileService = filesService ?? throw new ArgumentNullException(nameof(filesService));
	}

	[HttpPost]
	public async Task<IActionResult> CreateStory ( StoryVM storyVM )
	{
		int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
		var imageUploadPath = await _fileService.UploadImageAsync(storyVM.Image, ImageFileType.StoryImage);

		var newStory = new Story
		{
			UserId = userId,
			IsDeleted = false, // Default value for new stories
			CreatedAt = DateTime.UtcNow,
			ImageUrl = imageUploadPath
		};
		await _storiesService.CreateStoryAsync(newStory); // Use the service to create the story

		return RedirectToAction("Index", "Home"); // Redirect to the home page or wherever appropriate
	}
}
