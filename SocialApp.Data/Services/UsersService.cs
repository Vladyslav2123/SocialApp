using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public class UsersService : IUserService
{

	private readonly AppDbContext _context;

	public UsersService ( AppDbContext context )
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<User> GetUser ( int loggedInUserId )
	{
		return await _context.Users
			.Include(u => u.Posts)
			.Include(u => u.Stories)
			.Include(u => u.Likes)
			.Include(u => u.Comments)
			.Include(u => u.Favorites)
			.Include(u => u.Reports)
			.FirstOrDefaultAsync(u => u.Id == loggedInUserId && !u.IsDeleted)
			?? throw new KeyNotFoundException($"User with ID {loggedInUserId} not found or is deleted.");
	}
}
