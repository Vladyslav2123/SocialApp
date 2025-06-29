using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Services;

namespace SocialApp.Controllers;

public class SettingsController : Controller
{
	private readonly IUserService _userService;

	public SettingsController ( IUserService userService )
	{
		_userService = userService ?? throw new ArgumentNullException(nameof(userService));
	}

	public async Task<IActionResult> Index ()
	{
		int loggedInUserId = 1; // This should be replaced with actual logic to get the logged-in user's ID
		try
		{
			var user = await _userService.GetUser(loggedInUserId); // Use async/await in production code
			ViewBag.User = user;
		}
		catch (KeyNotFoundException ex)
		{
			ViewBag.ErrorMessage = ex.Message;
			return View("Error"); // Return an error view if user not found
		}
		catch (Exception ex)
		{
			ViewBag.ErrorMessage = "An unexpected error occurred: " + ex.Message;
			return View("Error"); // Return an error view for other exceptions
		}
		return View();
	}
}
