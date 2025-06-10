namespace SocialApp.Data.Models;

public class User
{
	public int Id { get; set; }
	public string Fullname { get; set; } = string.Empty;
	public string? ProfilePictureUrl { get; set; } = null;
}
