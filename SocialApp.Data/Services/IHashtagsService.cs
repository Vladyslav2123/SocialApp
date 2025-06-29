namespace SocialApp.Data.Services;

public interface IHashtagsService
{
	Task ProcessHashtagsForNewPostAsync ( string content );
	Task ProcessHashtagsForRemovedPostAsync ( string content );
}
