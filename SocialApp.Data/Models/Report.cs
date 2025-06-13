namespace SocialApp.Data.Models
{
	public class Report
	{
		public int Id { get; set; }
		//public string Reason { get; set; } // Reason for the report
		public DateTime CreatedAt { get; set; } // Timestamp when the report was created

		public int UserId { get; set; } // ID of the user who made the report
		public int PostId { get; set; } // ID of the post being reported

		// Navigation properties
		public User User { get; set; } // The user who made the report
		public Post Post { get; set; } // The post that is being reported
	}
}
