namespace SocialApp.Data.Models;

public class Hashtag
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty; // Hashtag name, e.g., "#example" without the hash symbol
	public int Count { get; set; } = 0; // Count of how many times this hashtag has been used
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp of when the hashtag was created
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Timestamp of when the hashtag was last updated
}
