using Microsoft.EntityFrameworkCore;
using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public class UsersService : IUsersService
{
	private readonly AppDbContext _appDbContext;
	public UsersService ( AppDbContext appDbContext )
	{
		_appDbContext = appDbContext;
	}

	public async Task<User> GetUser ( int loggedInUserId )
	{
		return await _appDbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId) ?? new User();
	}

	public async Task UpdateUserProfilePicture ( int loggedInUserId, string profilePictureUrl )
	{
		var userDb = await _appDbContext.Users.FirstOrDefaultAsync(n => n.Id == loggedInUserId);

		if (userDb != null)
		{
			userDb.ProfilePictureUrl = profilePictureUrl;
			_appDbContext.Users.Update(userDb);
			await _appDbContext.SaveChangesAsync();
		}
	}

	public async Task<List<Post>> GetUserPosts ( int userId )
	{
		var allPosts = await _appDbContext.Posts
			.Where(n => n.UserId == userId && n.Reports.Count < 5 && !n.IsDeleted)
			.Include(n => n.User)
			.Include(n => n.Likes)
			.Include(n => n.Favorites)
			.Include(n => n.Comments).ThenInclude(n => n.User)
			.Include(n => n.Reports)
			.OrderByDescending(n => n.CreatedAt)
			.ToListAsync();

		return allPosts;
	}
}
