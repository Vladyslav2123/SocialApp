namespace SocialApp.Data.Models;

public class Story
{
	public int Id { get; set; }

	public string? ImageUrl { get; set; } = null;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public bool IsDeleted { get; set; } = false;

	// Foreign key
	public int UserId { get; set; }

	// Navigation properties
	public User User { get; set; } = null!;
}
