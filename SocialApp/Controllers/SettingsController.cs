using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Helpers.Enums;
using SocialApp.Data.Services;
using SocialApp.ViewModels.Settings;

namespace SocialApp.Controllers;

[Authorize]
public class SettingsController : Controller
{
	private readonly IUsersService _userService;
	private readonly IFilesService _filesService; // Assuming you have a service for file handling

	public SettingsController ( IUsersService userService, IFilesService filesService )
	{
		_userService = userService ?? throw new ArgumentNullException(nameof(userService));
		_filesService = filesService ?? throw new ArgumentNullException(nameof(filesService));
	}

	public async Task<IActionResult> Index ()
	{
		int loggedInUserId = 1; // This should be replaced with actual logic to get the logged-in user's ID
		var user = await _userService.GetUser(loggedInUserId);

		return View(user);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateProfilePicture ( UpdateProfilePictureVM profilePictureVM )
	{
		var loggedInUserId = 1;
		var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, ImageFileType.ProfilePicture);

		await _userService.UpdateUserProfilePicture(loggedInUserId, uploadedProfilePictureUrl);
		return RedirectToAction("Index");
	}
}
