using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Helpers.Constants;
using SocialApp.Data.Models;
using SocialApp.ViewModels.Authentication;

namespace SocialApp.Controllers;

public class AuthenticationController : Controller
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;

	public AuthenticationController ( UserManager<User> userManager, SignInManager<User> signInManager )
	{
		// Constructor logic can go here if needed
		_userManager = userManager;
		_signInManager = signInManager;
	}

	public async Task<IActionResult> Login ()
	{
		return View();
	}

	public async Task<IActionResult> Register ()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Register ( RegisterVM registerVM )
	{
		var user = new User
		{
			UserName = registerVM.Email,
			Email = registerVM.Email,
			FullName = $"{registerVM.FirstName} {registerVM.LastName}"
		};

		var result = await _userManager.CreateAsync(user, registerVM.Password);
		if (result.Succeeded)
		{
			await _userManager.AddToRoleAsync(user, AppRoles.User);
			await _signInManager.SignInAsync(user, isPersistent: false);
			return RedirectToAction("Index", "Home");
		}

		return View();
	}
}
