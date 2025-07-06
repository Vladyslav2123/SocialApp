using System.ComponentModel.DataAnnotations;

namespace SocialApp.ViewModels.Authentication;

public class RegisterVM
{
	[Required(ErrorMessage = "First name is required.")]
	[StringLength(50, MinimumLength = 2,
		ErrorMessage = "First name cannot exceed 50 characters.")]
	[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
	public string FirstName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Last name is required.")]
	[StringLength(50, MinimumLength = 2,
		ErrorMessage = "First name cannot exceed 50 characters.")]
	[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
	public string LastName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Email is required.")]
	[EmailAddress(ErrorMessage = "Invalid email address format.")]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "Password is required.")]
	public string Password { get; set; } = string.Empty;

	[Required(ErrorMessage = "Confirm password is required.")]
	[Compare("Password", ErrorMessage = "Passwords do not match.")]
	public string ConfirmPassword { get; set; } = string.Empty;
}
