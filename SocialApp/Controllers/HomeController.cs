using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Data.Models;
using SocialApp.ViewModels.Home;

namespace SocialApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _dbContext;

		public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		public async Task<IActionResult> Index()
		{
			_logger.LogInformation("Index action called in HomeController.");

			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var allPosts = await _dbContext.Posts
				.Where(p => !p.IsPrivate || p.UserId == userId) // Fetch only public posts
				.Include(p => p.User) // Include the User entity to get user details for each post
				.Include(p => p.Likes) // Include Likes to get like counts and user likes
				.Include(p => p.Comments).ThenInclude(n => n.User) // Include Comments to get comments for each post
				.Include(p => p.Favorites) // Include Favorites to get favorite counts
				.OrderByDescending(p => p.CreatedAt) // Order posts by creation date, newest first
				.ToListAsync();

			return View(allPosts);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePost(PostVM post)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var newPost = new Post
			{
				Content = post.Content,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				ImageUrl = null, // Assuming no image is uploaded; handle image upload separately if needed
				NrOfReports = 0, // Default value for new posts
				UserId = userId,
			};

			// Handle image upload if provided
			if (post.Image != null && post.Image.Length > 0)
			{
				string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
				if (post.Image.ContentType.Contains("image"))
				{
					string rootPathImage = Path.Combine(rootPath, "images");
					Directory.CreateDirectory(rootPathImage); // Ensure the directory exists

					string fileName = $"{Guid.NewGuid()}_{post.Image.FileName}";
					string filePath = Path.Combine(rootPathImage, fileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
						await post.Image.CopyToAsync(fileStream);

					newPost.ImageUrl = $"/images/{fileName}"; // Set the image URL for the post
				}
			}

			_dbContext.Posts.Add(newPost);
			await _dbContext.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var existingLike = await _dbContext.Likes
				.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postLikeVM.PostId);

			if (existingLike != null)
			{
				// User has already liked the post, so we remove the like
				_dbContext.Likes.Remove(existingLike);
				await _dbContext.SaveChangesAsync();
			}
			else
			{
				// User has not liked the post, so we add a new like
				var newLike = new Like
				{
					UserId = userId,
					PostId = postLikeVM.PostId
				};
				await _dbContext.Likes.AddAsync(newLike);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var newComment = new Comment
			{
				Content = postCommentVM.Content,
				UserId = userId,
				PostId = postCommentVM.PostId,
				CreateAt = DateTime.UtcNow,
				UpdateAt = DateTime.UtcNow
			};

			await _dbContext.Comments.AddAsync(newComment);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
		{
			var comment = await _dbContext.Comments
				.FirstOrDefaultAsync(c => c.Id == removeCommentVM.CommentId);

			if (comment != null)
			{
				_dbContext.Comments.Remove(comment);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			var existingFavorite = await _dbContext.Favorites
				.FirstOrDefaultAsync(f => f.UserId == userId && f.PostId == postFavoriteVM.PostId);

			if (existingFavorite != null)
			{
				// User has already favorited the post, so we remove the favorite
				_dbContext.Favorites.Remove(existingFavorite);
				await _dbContext.SaveChangesAsync();
			}
			else
			{
				// User has not favorited the post, so we add a new favorite
				var newFavorite = new Favorite
				{
					UserId = userId,
					PostId = postFavoriteVM.PostId
				};
				await _dbContext.Favorites.AddAsync(newFavorite);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var post = await _dbContext.Posts
				.FirstOrDefaultAsync(p => p.Id == postVisibilityVM.PostId && p.UserId == userId);
			if (post != null)
			{
				post.IsPrivate = !post.IsPrivate; // Toggle the visibility
				post.UpdatedAt = DateTime.UtcNow; // Update the timestamp
				await _dbContext.SaveChangesAsync();
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var newReport = new Report
			{
				UserId = userId,
				PostId = postReportVM.PostId,
				CreatedAt = DateTime.UtcNow,
			};

			await _dbContext.Reports.AddAsync(newReport);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
