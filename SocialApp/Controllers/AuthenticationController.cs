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

	[HttpPost]
	public async Task<IActionResult> Login ( LoginVM loginVM )
	{

		if (!ModelState.IsValid)
		{
			return View(loginVM);
		}

		var user = await _userManager.FindByEmailAsync(loginVM.Email);

		if (user == null)
		{
			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return View(loginVM);
		}

		var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, isPersistent: false, lockoutOnFailure: false);

		if (result.Succeeded)
		{
			return RedirectToAction("Index", "Home");
		}

		ModelState.AddModelError(string.Empty, "Invalid login attempt.");
		return View(loginVM);
	}


	public async Task<IActionResult> Register ()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Register ( RegisterVM registerVM )
	{
		if (!ModelState.IsValid)
		{
			return View(registerVM);
		}

		// Create a new user instance
		var user = new User
		{
			UserName = registerVM.Email,
			Email = registerVM.Email,
			FullName = $"{registerVM.FirstName} {registerVM.LastName}"
		};

		// Check if the email is already registered
		var existingUser = await _userManager.FindByEmailAsync(registerVM.Email);
		if (existingUser != null)
		{
			ModelState.AddModelError(string.Empty, "Email is already registered.");
			return View(registerVM);
		}

		// Create the user with the provided password
		var result = await _userManager.CreateAsync(user, registerVM.Password);
		if (result.Succeeded)
		{
			await _userManager.AddToRoleAsync(user, AppRoles.User);
			await _signInManager.SignInAsync(user, isPersistent: false);
			return RedirectToAction("Index", "Home");
		}

		// If creation failed, add errors to the model state
		foreach (var error in result.Errors)
		{
			ModelState.AddModelError(string.Empty, error.Description);
		}

		return View(registerVM);
	}
}
