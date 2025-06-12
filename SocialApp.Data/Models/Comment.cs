namespace SocialApp.Data.Models
{
	public class Comment
	{
		public int Id { get; set; }

		public string Content { get; set; } = string.Empty; // Content of the comment

		public DateTime CreateAt { get; set; } = DateTime.UtcNow; // Timestamp for when the comment was created
		public DateTime UpdateAt { get; set; } = DateTime.UtcNow; // Timestamp for when the comment was last updated

		public int UserId { get; set; } // Foreign key to the User who liked the post
		public int PostId { get; set; } // Foreign key to the Post that was liked

		// Navigation properties
		public User User { get; set; } // Navigation property to the User entity
		public Post Post { get; set; } // Navigation property to the Post entity
	}
}
