using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public interface IUsersService
{
	Task<User> GetUser ( int loggedInUserId );
	Task UpdateUserProfilePicture ( int loggedInUserId, string profilePictureUrl );
	Task<List<Post>> GetUserPosts ( int userId );
}
