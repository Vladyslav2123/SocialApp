using Microsoft.EntityFrameworkCore;
using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public class StoriesService : IStoriesService
{
	private readonly AppDbContext _context;
	public StoriesService ( AppDbContext context )
	{
		_context = context;
	}

	public async Task<List<Story>> GetAllStoriesAsync ()
	{
		return await _context.Stories
			.Where(s => s.CreatedAt >= DateTime.UtcNow.AddDays(-1)) // Filter stories created in the last 24 hours
			.Where(s => !s.IsDeleted) // Filter out deleted stories
			.Include(s => s.User) // Include the User entity to get user details for each story
			.ToListAsync();
	}

	public async Task<Story> CreateStoryAsync ( Story story )
	{
		await _context.Stories.AddAsync(story);
		await _context.SaveChangesAsync();

		return story; // Return the created story with its ID and other properties set
	}
}
