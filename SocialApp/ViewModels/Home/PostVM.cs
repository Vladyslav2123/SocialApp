namespace SocialApp.ViewModels.Home;

public class PostVM
{
	public string Content { get; set; } = string.Empty;
	public IFormFile? Image { get; set; } = null; // Nullable to allow for no image upload
}
