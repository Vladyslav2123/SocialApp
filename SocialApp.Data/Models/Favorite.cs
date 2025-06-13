namespace SocialApp.Data.Models;

public class Favorite
{
	public int Id { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	public int UserId { get; set; }
	public int PostId { get; set; }

	// Navigation properties
	public User User { get; set; }
	public Post Post { get; set; }
}
