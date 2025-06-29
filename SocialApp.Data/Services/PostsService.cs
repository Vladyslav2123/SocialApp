using Microsoft.EntityFrameworkCore;
using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public class PostsService : IPostsService
{
	private readonly AppDbContext _context;

	public PostsService ( AppDbContext context )
	{
		_context = context;
	}

	public async Task AddPostCommentAsync ( Comment comment )
	{
		await _context.Comments.AddAsync(comment);
		await _context.SaveChangesAsync();
	}

	public async Task<Post> CreatePostAsync ( Post post )
	{
		await _context.Posts.AddAsync(post);
		await _context.SaveChangesAsync();

		return post;
	}

	public async Task<List<Post>> GetAllPostsAsync ( int loggedInUserId )
	{
		var allPosts = await _context.Posts
				.Where(p => (!p.IsPrivate || p.UserId == loggedInUserId) && p.Reports.Count < 5 && !p.IsDeleted) // Fetch only public posts
				.Include(p => p.User) // Include the User entity to get user details for each post
				.Include(p => p.Likes) // Include Likes to get like counts and user likes
				.Include(p => p.Comments).ThenInclude(n => n.User) // Include Comments to get comments for each post
				.Include(p => p.Favorites) // Include Favorites to get favorite counts
				.Include(p => p.Reports) // Include Reports to get report counts
				.OrderByDescending(p => p.CreatedAt) // Order posts by creation date, newest first
				.ToListAsync();

		return allPosts;
	}

	public async Task<Post> RemovePostAsync ( int postId )
	{
		var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

		if (post != null)
		{
			// _context.Posts.Remove(post);
			post.IsDeleted = true; // Soft delete the post by setting IsDeleted to true
			post.UpdatedAt = DateTime.UtcNow; // Update the timestamp to the current time
			_context.Posts.Update(post); // Update the post in the context
			await _context.SaveChangesAsync();
		}
		return post;
	}

	public async Task RemovePostCommentAsync ( int commentId )
	{
		var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
		if (comment != null)
		{
			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();
		}
	}

	public async Task ReportPostAsync ( int postId, int userId )
	{
		var newReport = new Report
		{
			PostId = postId,
			UserId = userId,
			CreatedAt = DateTime.UtcNow
		};

		await _context.Reports.AddAsync(newReport);
		await _context.SaveChangesAsync();
	}

	public async Task TogglePostFavoriteAsync ( int postId, int userId )
	{
		var existingFavorite = await _context.Favorites
			.Where(f => f.PostId == postId && f.UserId == userId)
				.FirstOrDefaultAsync();

		if (existingFavorite != null)
		{
			// User has already favorited the post, so we remove the favorite
			_context.Favorites.Remove(existingFavorite);
			await _context.SaveChangesAsync();
		}
		else
		{
			// User has not favorited the post, so we add a new favorite
			var newFavorite = new Favorite
			{
				UserId = userId,
				PostId = postId
			};
			await _context.Favorites.AddAsync(newFavorite);
			await _context.SaveChangesAsync();
		}
	}

	public async Task TogglePostLikeAsynk ( int postId, int userId )
	{
		var existingLike = await _context.Likes
			.Where(l => l.PostId == postId && l.UserId == userId)
				.FirstOrDefaultAsync();

		if (existingLike != null)
		{
			// User has already liked the post, so we remove the like
			_context.Likes.Remove(existingLike);
			await _context.SaveChangesAsync();
		}
		else
		{
			// User has not liked the post, so we add a new like
			var newLike = new Like
			{
				UserId = userId,
				PostId = postId
			};
			await _context.Likes.AddAsync(newLike);
			await _context.SaveChangesAsync();
		}
	}

	public async Task TogglePostVisibilityAsync ( int postId, int userId )
	{
		var post = await _context.Posts
			.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);
		if (post != null)
		{
			post.IsPrivate = !post.IsPrivate; // Toggle the visibility
			post.UpdatedAt = DateTime.UtcNow; // Update the timestamp
			_context.Posts.Update(post); // Update the post in the context
			await _context.SaveChangesAsync();
		}
	}
}
