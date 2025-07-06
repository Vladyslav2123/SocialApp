using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Helpers.Constants;
using SocialApp.Data.Models;
using SocialApp.ViewModels.Authentication;
using SocialApp.ViewModels.Settings;
using System.Security.Claims;

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
		var existingUserClaims = await _userManager.GetClaimsAsync(user);

		if (existingUserClaims.Any(c => c.Type == CustomClaim.FullName))
		{
			await _userManager.AddClaimAsync(user, new Claim(CustomClaim.FullName, user.FullName));
		}

		if (user == null)
		{
			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return View(loginVM);
		}

		var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVM.Password, isPersistent: false, lockoutOnFailure: false);

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

			await _userManager.AddClaimAsync(user, new Claim(CustomClaim.FullName, user.FullName));

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

	[Authorize]
	public async Task<IActionResult> Logout ()
	{
		await _signInManager.SignOutAsync();
		return RedirectToAction("Login");
	}

	[HttpPost]
	public async Task<IActionResult> UpdatePassword ( UpdatePasswordVM passwordVM )
	{
		if (passwordVM.NewPassword != passwordVM.ConfirmPassword)
		{
			TempData ["PasswordError"] = "New password and confirm password do not match.";
			TempData ["ActiveTab"] = "Password";
			return RedirectToAction("Index", "Settings");
		}

		var loggedInUser = await _userManager.GetUserAsync(User);

		var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(loggedInUser, passwordVM.CurrentPassword);

		if (!isCurrentPasswordValid)
		{
			TempData ["PasswordError"] = "Current password is incorrect.";
			TempData ["ActiveTab"] = "Password";
			return RedirectToAction("Index", "Settings");
		}

		var result = await _userManager.ChangePasswordAsync(loggedInUser, passwordVM.CurrentPassword, passwordVM.NewPassword);

		if (result.Succeeded)
		{
			TempData ["PasswordSuccess"] = "Password updated successfully.";
			TempData ["ActiveTab"] = "Password";
			await _signInManager.RefreshSignInAsync(loggedInUser);
		}

		return RedirectToAction("Index", "Settings");
	}

	[HttpPost]
	public async Task<IActionResult> UpdateProfile ( UpdateProfileVM profileVM )
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null)
		{
			return RedirectToAction("Login");
		}

		user.FullName = profileVM.FullName;
		user.UserName = profileVM.UserName;
		user.Bio = profileVM.Bio;

		// Update the user's claims if necessary
		var result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			TempData ["UserProfileError"] = "Failed to update profile. Please try again.";
			TempData ["ActiveTab"] = "Profile";
		}

		await _signInManager.RefreshSignInAsync(user);
		return RedirectToAction("Index", "Settings");
	}
}
