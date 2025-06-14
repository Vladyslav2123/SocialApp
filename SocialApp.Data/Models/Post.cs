using System.ComponentModel.DataAnnotations;

namespace SocialApp.Data.Models;

public class Post
{
	[Key]
	public int Id { get; set; }

	public string Content { get; set; } = string.Empty;

	public string? ImageUrl { get; set; } = null;

	public int NrOfReports { get; set; }

	public bool IsPrivate { get; set; } = false;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

	public bool IsDeleted { get; set; } = false;

	// Foreign key
	public int UserId { get; set; }

	// Navigation properties
	public User User { get; set; } = null!;
	public ICollection<Like> Likes { get; set; } = new List<Like>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
	public ICollection<Report> Reports { get; set; } = new List<Report>();
}
