using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public interface IStoriesService
{
	Task<List<Story>> GetAllStoriesAsync ();
	Task<Story> CreateStoryAsync ( Story story );
}
