namespace SocialApp.Data.Models;

public class User
{
	public int Id { get; set; }
	public string Fullname { get; set; } = string.Empty;
	public string? ProfilePictureUrl { get; set; } = null;

	public ICollection<Post> Posts { get; set; } = new List<Post>();
	public ICollection<Like> Likes { get; set; } = new List<Like>();
}
