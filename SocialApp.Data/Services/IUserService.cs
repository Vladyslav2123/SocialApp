using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public interface IUserService
{
	Task<User> GetUser ( int loggedInUserId );
}
