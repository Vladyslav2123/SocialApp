using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public interface IPostsService
{
	Task<List<Post>> GetAllPostsAsync ( int loggedInUserId );
	Task<Post> GetPostByIdAsync ( int postId );
	Task<List<Post>> GetAllFavoritedPostsAsync ( int loggedInUserId );

	Task<Post> CreatePostAsync ( Post post );
	Task<Post> RemovePostAsync ( int postId );

	Task AddPostCommentAsync ( Comment comment );
	Task RemovePostCommentAsync ( int commentId );

	Task TogglePostLikeAsynk ( int postId, int userId );
	Task TogglePostFavoriteAsync ( int postId, int userId );
	Task TogglePostVisibilityAsync ( int postId, int userId );
	Task ReportPostAsync ( int postId, int userId );
}
