namespace SocialApp.Data.Models
{
	public class Like
	{
		public int Id { get; set; }
		public int UserId { get; set; } // Foreign key to the User who liked the post
		public int PostId { get; set; } // Foreign key to the Post that was liked

		// Navigation properties
		public User User { get; set; } // Navigation property to the User entity
		public Post Post { get; set; } // Navigation property to the Post entity
	}
}
